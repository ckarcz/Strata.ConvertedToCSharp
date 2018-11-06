/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.schedule
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P2M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_18;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;

	/// <summary>
	/// Test <seealso cref="SchedulePeriod"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SchedulePeriodTest
	public class SchedulePeriodTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate JUN_15 = date(2014, JUNE, 15); // Sunday
	  private static readonly LocalDate JUN_16 = date(2014, JUNE, 16);
	  private static readonly LocalDate JUN_17 = date(2014, JUNE, 17);
	  private static readonly LocalDate JUN_18 = date(2014, JUNE, 18);
	  private static readonly LocalDate JUL_04 = date(2014, JULY, 4);
	  private static readonly LocalDate JUL_05 = date(2014, JULY, 5);
	  private static readonly LocalDate JUL_17 = date(2014, JULY, 17);
	  private static readonly LocalDate JUL_18 = date(2014, JULY, 18);
	  private static readonly LocalDate AUG_17 = date(2014, AUGUST, 17); // Sunday
	  private static readonly LocalDate AUG_18 = date(2014, AUGUST, 18); // Monday
	  private static readonly LocalDate SEP_17 = date(2014, SEPTEMBER, 17);
	  private const double TOLERANCE = 1.0E-6;

	  //-------------------------------------------------------------------------
	  public virtual void test_of_null()
	  {
		assertThrowsIllegalArg(() => SchedulePeriod.of(null, JUL_18, JUL_04, JUL_17));
		assertThrowsIllegalArg(() => SchedulePeriod.of(JUL_05, null, JUL_04, JUL_17));
		assertThrowsIllegalArg(() => SchedulePeriod.of(JUL_05, JUL_18, null, JUL_17));
		assertThrowsIllegalArg(() => SchedulePeriod.of(JUL_05, JUL_18, JUL_04, null));
		assertThrowsIllegalArg(() => SchedulePeriod.of(null, null, null, null));
	  }

	  public virtual void test_of_all()
	  {
		SchedulePeriod test = SchedulePeriod.of(JUL_05, JUL_18, JUL_04, JUL_17);
		assertEquals(test.StartDate, JUL_05);
		assertEquals(test.EndDate, JUL_18);
		assertEquals(test.UnadjustedStartDate, JUL_04);
		assertEquals(test.UnadjustedEndDate, JUL_17);
	  }

	  public virtual void test_of_noUnadjusted()
	  {
		SchedulePeriod test = SchedulePeriod.of(JUL_05, JUL_18);
		assertEquals(test.StartDate, JUL_05);
		assertEquals(test.EndDate, JUL_18);
		assertEquals(test.UnadjustedStartDate, JUL_05);
		assertEquals(test.UnadjustedEndDate, JUL_18);
	  }

	  public virtual void test_builder_defaults()
	  {
		SchedulePeriod test = SchedulePeriod.builder().startDate(JUL_05).endDate(JUL_18).build();
		assertEquals(test.StartDate, JUL_05);
		assertEquals(test.EndDate, JUL_18);
		assertEquals(test.UnadjustedStartDate, JUL_05);
		assertEquals(test.UnadjustedEndDate, JUL_18);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_yearFraction()
	  {
		SchedulePeriod test = SchedulePeriod.of(JUN_16, JUL_18, JUN_16, JUL_17);
		Schedule schedule = Schedule.ofTerm(test);
		assertEquals(test.yearFraction(DayCounts.ACT_360, schedule), DayCounts.ACT_360.yearFraction(JUN_16, JUL_18, schedule), TOLERANCE);
	  }

	  public virtual void test_yearFraction_null()
	  {
		SchedulePeriod test = SchedulePeriod.of(JUN_16, JUL_18, JUN_16, JUL_17);
		Schedule schedule = Schedule.ofTerm(test);
		assertThrowsIllegalArg(() => test.yearFraction(null, schedule));
		assertThrowsIllegalArg(() => test.yearFraction(DayCounts.ACT_360, null));
		assertThrowsIllegalArg(() => test.yearFraction(null, null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_length()
	  {
		assertEquals(SchedulePeriod.of(JUN_16, JUN_18, JUN_16, JUN_18).length(), Period.between(JUN_16, JUN_18));
		assertEquals(SchedulePeriod.of(JUN_16, JUL_18, JUN_16, JUL_17).length(), Period.between(JUN_16, JUL_18));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_lengthInDays()
	  {
		assertEquals(SchedulePeriod.of(JUN_16, JUN_18, JUN_16, JUN_18).lengthInDays(), 2);
		assertEquals(SchedulePeriod.of(JUN_16, JUL_18, JUN_16, JUL_17).lengthInDays(), 32);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_isRegular()
	  {
		assertEquals(SchedulePeriod.of(JUN_18, JUL_18).isRegular(P1M, DAY_18), true);
		assertEquals(SchedulePeriod.of(JUN_18, JUL_05).isRegular(P1M, DAY_18), false);
		assertEquals(SchedulePeriod.of(JUL_05, JUL_18).isRegular(P1M, DAY_18), false);
		assertEquals(SchedulePeriod.of(JUN_18, JUL_05).isRegular(P2M, DAY_18), false);
	  }

	  public virtual void test_isRegular_null()
	  {
		SchedulePeriod test = SchedulePeriod.of(JUN_16, JUL_18);
		assertThrowsIllegalArg(() => test.isRegular(null, DAY_18));
		assertThrowsIllegalArg(() => test.isRegular(P1M, null));
		assertThrowsIllegalArg(() => test.isRegular(null, null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_contains()
	  {
		assertEquals(SchedulePeriod.of(JUN_16, JUL_18, JUN_16, JUL_17).contains(JUN_15), false);
		assertEquals(SchedulePeriod.of(JUN_16, JUL_18, JUN_16, JUL_17).contains(JUN_16), true);
		assertEquals(SchedulePeriod.of(JUN_16, JUL_18, JUN_16, JUL_17).contains(JUL_05), true);
		assertEquals(SchedulePeriod.of(JUN_16, JUL_18, JUN_16, JUL_17).contains(JUL_17), true);
		assertEquals(SchedulePeriod.of(JUN_16, JUL_18, JUN_16, JUL_17).contains(JUL_18), false);
	  }

	  public virtual void test_contains_null()
	  {
		SchedulePeriod test = SchedulePeriod.of(JUN_16, JUL_18);
		assertThrowsIllegalArg(() => test.contains(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_subSchedule_1monthIn3Month()
	  {
		SchedulePeriod test = SchedulePeriod.of(JUN_17, SEP_17);
		Schedule schedule = test.subSchedule(P1M, RollConventions.DAY_17, StubConvention.NONE, BusinessDayAdjustment.NONE).createSchedule(REF_DATA);
		assertEquals(schedule.size(), 3);
		assertEquals(schedule.getPeriod(0), SchedulePeriod.of(JUN_17, JUL_17));
		assertEquals(schedule.getPeriod(1), SchedulePeriod.of(JUL_17, AUG_17));
		assertEquals(schedule.getPeriod(2), SchedulePeriod.of(AUG_17, SEP_17));
		assertEquals(schedule.Frequency, P1M);
		assertEquals(schedule.RollConvention, RollConventions.DAY_17);
	  }

	  public virtual void test_subSchedule_3monthIn3Month()
	  {
		SchedulePeriod test = SchedulePeriod.of(JUN_17, SEP_17);
		Schedule schedule = test.subSchedule(P3M, RollConventions.DAY_17, StubConvention.NONE, BusinessDayAdjustment.NONE).createSchedule(REF_DATA);
		assertEquals(schedule.size(), 1);
		assertEquals(schedule.getPeriod(0), SchedulePeriod.of(JUN_17, SEP_17));
	  }

	  public virtual void test_subSchedule_2monthIn3Month_shortInitial()
	  {
		SchedulePeriod test = SchedulePeriod.of(JUN_17, SEP_17);
		Schedule schedule = test.subSchedule(P2M, RollConventions.DAY_17, StubConvention.SHORT_INITIAL, BusinessDayAdjustment.NONE).createSchedule(REF_DATA);
		assertEquals(schedule.size(), 2);
		assertEquals(schedule.getPeriod(0), SchedulePeriod.of(JUN_17, JUL_17));
		assertEquals(schedule.getPeriod(1), SchedulePeriod.of(JUL_17, SEP_17));
		assertEquals(schedule.Frequency, P2M);
		assertEquals(schedule.RollConvention, RollConventions.DAY_17);
	  }

	  public virtual void test_subSchedule_2monthIn3Month_shortFinal()
	  {
		SchedulePeriod test = SchedulePeriod.of(JUN_17, SEP_17);
		Schedule schedule = test.subSchedule(P2M, RollConventions.DAY_17, StubConvention.SHORT_FINAL, BusinessDayAdjustment.NONE).createSchedule(REF_DATA);
		assertEquals(schedule.size(), 2);
		assertEquals(schedule.getPeriod(0), SchedulePeriod.of(JUN_17, AUG_17));
		assertEquals(schedule.getPeriod(1), SchedulePeriod.of(AUG_17, SEP_17));
		assertEquals(schedule.Frequency, P2M);
		assertEquals(schedule.RollConvention, RollConventions.DAY_17);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toAdjusted()
	  {
		SchedulePeriod test1 = SchedulePeriod.of(JUN_15, SEP_17);
		assertEquals(test1.toAdjusted(date => date), test1);
		assertEquals(test1.toAdjusted(date => date.Equals(JUN_15) ? JUN_16 : date), SchedulePeriod.of(JUN_16, SEP_17, JUN_15, SEP_17));
		SchedulePeriod test2 = SchedulePeriod.of(JUN_16, AUG_17);
		assertEquals(test2.toAdjusted(date => date.Equals(AUG_17) ? AUG_18 : date), SchedulePeriod.of(JUN_16, AUG_18, JUN_16, AUG_17));
	  }

	  public virtual void test_toUnadjusted()
	  {
		assertEquals(SchedulePeriod.of(JUN_15, SEP_17).toUnadjusted(), SchedulePeriod.of(JUN_15, SEP_17));
		assertEquals(SchedulePeriod.of(JUN_16, SEP_17, JUN_15, SEP_17).toUnadjusted(), SchedulePeriod.of(JUN_15, SEP_17));
		assertEquals(SchedulePeriod.of(JUN_16, JUL_18, JUN_16, JUL_17).toUnadjusted(), SchedulePeriod.of(JUN_16, JUL_17));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compareTo()
	  {
		SchedulePeriod a = SchedulePeriod.of(JUL_05, JUL_18);
		SchedulePeriod b = SchedulePeriod.of(JUL_04, JUL_18);
		SchedulePeriod c = SchedulePeriod.of(JUL_05, JUL_17);
		assertEquals(a.CompareTo(a) == 0, true);
		assertEquals(a.CompareTo(b) > 0, true);
		assertEquals(a.CompareTo(c) > 0, true);

		assertEquals(b.CompareTo(a) < 0, true);
		assertEquals(b.CompareTo(b) == 0, true);
		assertEquals(b.CompareTo(c) < 0, true);

		assertEquals(c.CompareTo(a) < 0, true);
		assertEquals(c.CompareTo(b) > 0, true);
		assertEquals(c.CompareTo(c) == 0, true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage_equals()
	  {
		SchedulePeriod a1 = SchedulePeriod.of(JUL_05, JUL_18, JUL_04, JUL_17);
		SchedulePeriod a2 = SchedulePeriod.of(JUL_05, JUL_18, JUL_04, JUL_17);
		SchedulePeriod b = SchedulePeriod.of(JUL_04, JUL_18, JUL_04, JUL_17);
		SchedulePeriod c = SchedulePeriod.of(JUL_05, JUL_17, JUL_04, JUL_17);
		SchedulePeriod d = SchedulePeriod.of(JUL_05, JUL_18, JUL_05, JUL_17);
		SchedulePeriod e = SchedulePeriod.of(JUL_05, JUL_18, JUL_04, JUL_18);
		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(c), false);
		assertEquals(a1.Equals(d), false);
		assertEquals(a1.Equals(e), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage_builder()
	  {
		SchedulePeriod.Builder builder = SchedulePeriod.builder();
		builder.startDate(JUL_05).endDate(JUL_18).unadjustedStartDate(JUL_04).unadjustedEndDate(JUL_17).build();
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SchedulePeriod test = SchedulePeriod.of(JUL_05, JUL_18, JUL_04, JUL_17);
		coverImmutableBean(test);
	  }

	  public virtual void test_serialization()
	  {
		SchedulePeriod test = SchedulePeriod.of(JUL_05, JUL_18, JUL_04, JUL_17);
		assertSerialization(test);
	  }

	}

}