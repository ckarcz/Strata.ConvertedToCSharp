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
//	import static com.opengamma.strata.basics.schedule.Frequency.TERM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_17;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Test <seealso cref="Schedule"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ScheduleTest
	public class ScheduleTest
	{

	  private static readonly LocalDate JUN_15 = date(2014, JUNE, 15);
	  private static readonly LocalDate JUN_16 = date(2014, JUNE, 16);
	  private static readonly LocalDate JUL_03 = date(2014, JULY, 3);
	  private static readonly LocalDate JUL_04 = date(2014, JULY, 4);
	  private static readonly LocalDate JUL_16 = date(2014, JULY, 16);
	  private static readonly LocalDate JUL_17 = date(2014, JULY, 17);
	  private static readonly LocalDate AUG_16 = date(2014, AUGUST, 16);
	  private static readonly LocalDate AUG_17 = date(2014, AUGUST, 17);
	  private static readonly LocalDate SEP_17 = date(2014, SEPTEMBER, 17);
	  private static readonly LocalDate SEP_30 = date(2014, SEPTEMBER, 30);
	  private static readonly LocalDate OCT_15 = date(2014, OCTOBER, 15);
	  private static readonly LocalDate OCT_17 = date(2014, OCTOBER, 17);
	  private static readonly LocalDate NOV_17 = date(2014, NOVEMBER, 17);
	  private static readonly LocalDate DEC_17 = date(2014, DECEMBER, 17);

	  private static readonly SchedulePeriod P1_STUB = SchedulePeriod.of(JUL_03, JUL_17, JUL_04, JUL_17);
	  private static readonly SchedulePeriod P2_NORMAL = SchedulePeriod.of(JUL_17, AUG_16, JUL_17, AUG_17);
	  private static readonly SchedulePeriod P3_NORMAL = SchedulePeriod.of(AUG_16, SEP_17, AUG_17, SEP_17);
	  private static readonly SchedulePeriod P4_STUB = SchedulePeriod.of(SEP_17, SEP_30);
	  private static readonly SchedulePeriod P4_NORMAL = SchedulePeriod.of(SEP_17, OCT_17);
	  private static readonly SchedulePeriod P5_NORMAL = SchedulePeriod.of(OCT_17, NOV_17);
	  private static readonly SchedulePeriod P6_NORMAL = SchedulePeriod.of(NOV_17, DEC_17);

	  private static readonly SchedulePeriod P1_2 = SchedulePeriod.of(JUL_03, AUG_16, JUL_04, AUG_17);
	  private static readonly SchedulePeriod P1_3 = SchedulePeriod.of(JUL_03, SEP_17, JUL_04, SEP_17);
	  private static readonly SchedulePeriod P2_3 = SchedulePeriod.of(JUL_17, SEP_17);
	  private static readonly SchedulePeriod P3_4 = SchedulePeriod.of(AUG_16, OCT_17, AUG_17, OCT_17);
	  private static readonly SchedulePeriod P3_4STUB = SchedulePeriod.of(AUG_16, SEP_30, AUG_17, SEP_30);
	  private static readonly SchedulePeriod P4_5 = SchedulePeriod.of(SEP_17, NOV_17);
	  private static readonly SchedulePeriod P5_6 = SchedulePeriod.of(OCT_17, DEC_17);

	  private static readonly SchedulePeriod P2_4 = SchedulePeriod.of(JUL_17, OCT_17);
	  private static readonly SchedulePeriod P4_6 = SchedulePeriod.of(SEP_17, DEC_17);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_size0()
	  {
		assertThrowsIllegalArg(() => Schedule.builder().periods(ImmutableList.of()));
	  }

	  public virtual void test_ofTerm()
	  {
		Schedule test = Schedule.ofTerm(P1_STUB);
		assertEquals(test.size(), 1);
		assertEquals(test.Term, true);
		assertEquals(test.SinglePeriod, true);
		assertEquals(test.Frequency, TERM);
		assertEquals(test.RollConvention, RollConventions.NONE);
		assertEquals(test.EndOfMonthConvention, false);
		assertEquals(test.Periods, ImmutableList.of(P1_STUB));
		assertEquals(test.getPeriod(0), P1_STUB);
		assertEquals(test.StartDate, P1_STUB.StartDate);
		assertEquals(test.EndDate, P1_STUB.EndDate);
		assertEquals(test.UnadjustedStartDate, P1_STUB.UnadjustedStartDate);
		assertEquals(test.UnadjustedEndDate, P1_STUB.UnadjustedEndDate);
		assertEquals(test.FirstPeriod, P1_STUB);
		assertEquals(test.LastPeriod, P1_STUB);
		assertEquals(test.InitialStub, null);
		assertEquals(test.FinalStub, null);
		assertEquals(test.RegularPeriods, ImmutableList.of(P1_STUB));
		assertThrows(() => test.getPeriod(1), typeof(System.IndexOutOfRangeException));
		assertEquals(test.UnadjustedDates, ImmutableList.of(JUL_04, JUL_17));
	  }

	  public virtual void test_size1_stub()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P1_STUB)).frequency(P1M).rollConvention(DAY_17).build();
		assertEquals(test.size(), 1);
		assertEquals(test.Term, false);
		assertEquals(test.SinglePeriod, true);
		assertEquals(test.Frequency, P1M);
		assertEquals(test.RollConvention, DAY_17);
		assertEquals(test.EndOfMonthConvention, false);
		assertEquals(test.Periods, ImmutableList.of(P1_STUB));
		assertEquals(test.getPeriod(0), P1_STUB);
		assertEquals(test.StartDate, P1_STUB.StartDate);
		assertEquals(test.EndDate, P1_STUB.EndDate);
		assertEquals(test.UnadjustedStartDate, P1_STUB.UnadjustedStartDate);
		assertEquals(test.UnadjustedEndDate, P1_STUB.UnadjustedEndDate);
		assertEquals(test.FirstPeriod, P1_STUB);
		assertEquals(test.LastPeriod, P1_STUB);
		assertEquals(test.InitialStub, P1_STUB);
		assertEquals(test.FinalStub, null);
		assertEquals(test.RegularPeriods, ImmutableList.of());
		assertThrows(() => test.getPeriod(1), typeof(System.IndexOutOfRangeException));
		assertEquals(test.UnadjustedDates, ImmutableList.of(JUL_04, JUL_17));
	  }

	  public virtual void test_size1_noStub()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		assertEquals(test.size(), 1);
		assertEquals(test.Term, false);
		assertEquals(test.SinglePeriod, true);
		assertEquals(test.Frequency, P1M);
		assertEquals(test.RollConvention, DAY_17);
		assertEquals(test.EndOfMonthConvention, false);
		assertEquals(test.Periods, ImmutableList.of(P2_NORMAL));
		assertEquals(test.getPeriod(0), P2_NORMAL);
		assertEquals(test.StartDate, P2_NORMAL.StartDate);
		assertEquals(test.EndDate, P2_NORMAL.EndDate);
		assertEquals(test.UnadjustedStartDate, P2_NORMAL.UnadjustedStartDate);
		assertEquals(test.UnadjustedEndDate, P2_NORMAL.UnadjustedEndDate);
		assertEquals(test.FirstPeriod, P2_NORMAL);
		assertEquals(test.LastPeriod, P2_NORMAL);
		assertEquals(test.InitialStub, null);
		assertEquals(test.FinalStub, null);
		assertEquals(test.RegularPeriods, ImmutableList.of(P2_NORMAL));
		assertThrows(() => test.getPeriod(1), typeof(System.IndexOutOfRangeException));
		assertEquals(test.UnadjustedDates, ImmutableList.of(JUL_17, AUG_17));
	  }

	  public virtual void test_of_size2_initialStub()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P1_STUB, P2_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		assertEquals(test.size(), 2);
		assertEquals(test.Term, false);
		assertEquals(test.SinglePeriod, false);
		assertEquals(test.Frequency, P1M);
		assertEquals(test.RollConvention, DAY_17);
		assertEquals(test.EndOfMonthConvention, false);
		assertEquals(test.Periods, ImmutableList.of(P1_STUB, P2_NORMAL));
		assertEquals(test.getPeriod(0), P1_STUB);
		assertEquals(test.getPeriod(1), P2_NORMAL);
		assertEquals(test.StartDate, P1_STUB.StartDate);
		assertEquals(test.EndDate, P2_NORMAL.EndDate);
		assertEquals(test.UnadjustedStartDate, P1_STUB.UnadjustedStartDate);
		assertEquals(test.UnadjustedEndDate, P2_NORMAL.UnadjustedEndDate);
		assertEquals(test.FirstPeriod, P1_STUB);
		assertEquals(test.LastPeriod, P2_NORMAL);
		assertEquals(test.InitialStub, P1_STUB);
		assertEquals(test.FinalStub, null);
		assertEquals(test.RegularPeriods, ImmutableList.of(P2_NORMAL));
		assertThrows(() => test.getPeriod(2), typeof(System.IndexOutOfRangeException));
		assertEquals(test.UnadjustedDates, ImmutableList.of(JUL_04, JUL_17, AUG_17));
	  }

	  public virtual void test_of_size2_noStub()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		assertEquals(test.size(), 2);
		assertEquals(test.Term, false);
		assertEquals(test.SinglePeriod, false);
		assertEquals(test.Frequency, P1M);
		assertEquals(test.RollConvention, DAY_17);
		assertEquals(test.EndOfMonthConvention, false);
		assertEquals(test.Periods, ImmutableList.of(P2_NORMAL, P3_NORMAL));
		assertEquals(test.getPeriod(0), P2_NORMAL);
		assertEquals(test.getPeriod(1), P3_NORMAL);
		assertEquals(test.StartDate, P2_NORMAL.StartDate);
		assertEquals(test.EndDate, P3_NORMAL.EndDate);
		assertEquals(test.UnadjustedStartDate, P2_NORMAL.UnadjustedStartDate);
		assertEquals(test.UnadjustedEndDate, P3_NORMAL.UnadjustedEndDate);
		assertEquals(test.FirstPeriod, P2_NORMAL);
		assertEquals(test.LastPeriod, P3_NORMAL);
		assertEquals(test.InitialStub, null);
		assertEquals(test.FinalStub, null);
		assertEquals(test.RegularPeriods, ImmutableList.of(P2_NORMAL, P3_NORMAL));
		assertThrows(() => test.getPeriod(2), typeof(System.IndexOutOfRangeException));
		assertEquals(test.UnadjustedDates, ImmutableList.of(JUL_17, AUG_17, SEP_17));
	  }

	  public virtual void test_of_size2_finalStub()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P3_NORMAL, P4_STUB)).frequency(P1M).rollConvention(DAY_17).build();
		assertEquals(test.size(), 2);
		assertEquals(test.Term, false);
		assertEquals(test.SinglePeriod, false);
		assertEquals(test.Frequency, P1M);
		assertEquals(test.RollConvention, DAY_17);
		assertEquals(test.EndOfMonthConvention, false);
		assertEquals(test.Periods, ImmutableList.of(P3_NORMAL, P4_STUB));
		assertEquals(test.getPeriod(0), P3_NORMAL);
		assertEquals(test.getPeriod(1), P4_STUB);
		assertEquals(test.StartDate, P3_NORMAL.StartDate);
		assertEquals(test.EndDate, P4_STUB.EndDate);
		assertEquals(test.UnadjustedStartDate, P3_NORMAL.UnadjustedStartDate);
		assertEquals(test.UnadjustedEndDate, P4_STUB.UnadjustedEndDate);
		assertEquals(test.FirstPeriod, P3_NORMAL);
		assertEquals(test.LastPeriod, P4_STUB);
		assertEquals(test.InitialStub, null);
		assertEquals(test.FinalStub, P4_STUB);
		assertEquals(test.RegularPeriods, ImmutableList.of(P3_NORMAL));
		assertThrows(() => test.getPeriod(2), typeof(System.IndexOutOfRangeException));
		assertEquals(test.UnadjustedDates, ImmutableList.of(AUG_17, SEP_17, SEP_30));
	  }

	  public virtual void test_of_size3_initialStub()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P1_STUB, P2_NORMAL, P3_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		assertEquals(test.size(), 3);
		assertEquals(test.Term, false);
		assertEquals(test.SinglePeriod, false);
		assertEquals(test.Frequency, P1M);
		assertEquals(test.RollConvention, DAY_17);
		assertEquals(test.EndOfMonthConvention, false);
		assertEquals(test.Periods, ImmutableList.of(P1_STUB, P2_NORMAL, P3_NORMAL));
		assertEquals(test.getPeriod(0), P1_STUB);
		assertEquals(test.getPeriod(1), P2_NORMAL);
		assertEquals(test.getPeriod(2), P3_NORMAL);
		assertEquals(test.StartDate, P1_STUB.StartDate);
		assertEquals(test.EndDate, P3_NORMAL.EndDate);
		assertEquals(test.UnadjustedStartDate, P1_STUB.UnadjustedStartDate);
		assertEquals(test.UnadjustedEndDate, P3_NORMAL.UnadjustedEndDate);
		assertEquals(test.FirstPeriod, P1_STUB);
		assertEquals(test.LastPeriod, P3_NORMAL);
		assertEquals(test.InitialStub, P1_STUB);
		assertEquals(test.FinalStub, null);
		assertEquals(test.RegularPeriods, ImmutableList.of(P2_NORMAL, P3_NORMAL));
		assertThrows(() => test.getPeriod(3), typeof(System.IndexOutOfRangeException));
		assertEquals(test.UnadjustedDates, ImmutableList.of(JUL_04, JUL_17, AUG_17, SEP_17));
	  }

	  public virtual void test_of_size4_bothStubs()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P1_STUB, P2_NORMAL, P3_NORMAL, P4_STUB)).frequency(P1M).rollConvention(DAY_17).build();
		assertEquals(test.size(), 4);
		assertEquals(test.Term, false);
		assertEquals(test.SinglePeriod, false);
		assertEquals(test.Frequency, P1M);
		assertEquals(test.RollConvention, DAY_17);
		assertEquals(test.EndOfMonthConvention, false);
		assertEquals(test.Periods, ImmutableList.of(P1_STUB, P2_NORMAL, P3_NORMAL, P4_STUB));
		assertEquals(test.getPeriod(0), P1_STUB);
		assertEquals(test.getPeriod(1), P2_NORMAL);
		assertEquals(test.getPeriod(2), P3_NORMAL);
		assertEquals(test.getPeriod(3), P4_STUB);
		assertEquals(test.StartDate, P1_STUB.StartDate);
		assertEquals(test.EndDate, P4_STUB.EndDate);
		assertEquals(test.UnadjustedStartDate, P1_STUB.UnadjustedStartDate);
		assertEquals(test.UnadjustedEndDate, P4_STUB.UnadjustedEndDate);
		assertEquals(test.FirstPeriod, P1_STUB);
		assertEquals(test.LastPeriod, P4_STUB);
		assertEquals(test.InitialStub, P1_STUB);
		assertEquals(test.FinalStub, P4_STUB);
		assertEquals(test.RegularPeriods, ImmutableList.of(P2_NORMAL, P3_NORMAL));
		assertThrows(() => test.getPeriod(4), typeof(System.IndexOutOfRangeException));
		assertEquals(test.UnadjustedDates, ImmutableList.of(JUL_04, JUL_17, AUG_17, SEP_17, SEP_30));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_isEndOfMonthConvention_eom()
	  {
		// schedule doesn't make sense, but test only requires roll convention of EOM
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL)).frequency(P1M).rollConvention(RollConventions.EOM).build();
		assertEquals(test.EndOfMonthConvention, true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_getPeriodEndDate()
	  {
		// schedule doesn't make sense, but test only requires roll convention of EOM
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		assertEquals(test.getPeriodEndDate(P2_NORMAL.StartDate), P2_NORMAL.EndDate);
		assertEquals(test.getPeriodEndDate(P2_NORMAL.StartDate.plusDays(1)), P2_NORMAL.EndDate);
		assertEquals(test.getPeriodEndDate(P3_NORMAL.StartDate), P3_NORMAL.EndDate);
		assertEquals(test.getPeriodEndDate(P3_NORMAL.StartDate.plusDays(1)), P3_NORMAL.EndDate);
		assertThrowsIllegalArg(() => test.getPeriodEndDate(P2_NORMAL.StartDate.minusDays(1)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_mergeToTerm()
	  {
		Schedule testNormal = Schedule.builder().periods(ImmutableList.of(P1_STUB, P2_NORMAL, P3_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		assertEquals(testNormal.mergeToTerm(), Schedule.ofTerm(P1_3));
		assertEquals(testNormal.mergeToTerm().mergeToTerm(), Schedule.ofTerm(P1_3));
	  }

	  public virtual void test_mergeToTerm_size1_stub()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P1_STUB)).frequency(P1M).rollConvention(DAY_17).build();
		assertEquals(test.mergeToTerm(), Schedule.ofTerm(P1_STUB));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_merge_group2_within2_initialStub()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P1_STUB, P2_NORMAL, P3_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		Schedule expected = Schedule.builder().periods(ImmutableList.of(P1_STUB, P2_3)).frequency(P2M).rollConvention(DAY_17).build();
		assertEquals(test.mergeRegular(2, true), expected);
		assertEquals(test.mergeRegular(2, false), expected);
		assertEquals(test.merge(2, P2_NORMAL.UnadjustedStartDate, P3_NORMAL.UnadjustedEndDate), expected);
		assertEquals(test.merge(2, P2_NORMAL.StartDate, P3_NORMAL.EndDate), expected);
	  }

	  public virtual void test_merge_group2_within2_noStub()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		Schedule expected = Schedule.builder().periods(ImmutableList.of(P2_3)).frequency(P2M).rollConvention(DAY_17).build();
		assertEquals(test.mergeRegular(2, true), expected);
		assertEquals(test.mergeRegular(2, false), expected);
		assertEquals(test.merge(2, P2_NORMAL.UnadjustedStartDate, P3_NORMAL.UnadjustedEndDate), expected);
		assertEquals(test.merge(2, P2_NORMAL.StartDate, P3_NORMAL.EndDate), expected);
	  }

	  public virtual void test_merge_group2_within2_finalStub()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL, P4_STUB)).frequency(P1M).rollConvention(DAY_17).build();
		Schedule expected = Schedule.builder().periods(ImmutableList.of(P2_3, P4_STUB)).frequency(P2M).rollConvention(DAY_17).build();
		assertEquals(test.mergeRegular(2, true), expected);
		assertEquals(test.mergeRegular(2, false), expected);
		assertEquals(test.merge(2, P2_NORMAL.UnadjustedStartDate, P3_NORMAL.UnadjustedEndDate), expected);
		assertEquals(test.merge(2, P2_NORMAL.StartDate, P3_NORMAL.EndDate), expected);
	  }

	  public virtual void test_merge_group2_within3_forwards()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL, P4_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		Schedule expected = Schedule.builder().periods(ImmutableList.of(P2_3, P4_NORMAL)).frequency(P2M).rollConvention(DAY_17).build();
		assertEquals(test.mergeRegular(2, true), expected);
		assertEquals(test.merge(2, P2_NORMAL.UnadjustedStartDate, P3_NORMAL.UnadjustedEndDate), expected);
		assertEquals(test.merge(2, P2_NORMAL.StartDate, P3_NORMAL.EndDate), expected);
	  }

	  public virtual void test_merge_group2_within3_backwards()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL, P4_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		Schedule expected = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_4)).frequency(P2M).rollConvention(DAY_17).build();
		assertEquals(test.mergeRegular(2, false), expected);
		assertEquals(test.merge(2, P3_NORMAL.UnadjustedStartDate, P4_NORMAL.UnadjustedEndDate), expected);
		assertEquals(test.merge(2, P3_NORMAL.StartDate, P4_NORMAL.EndDate), expected);
	  }

	  public virtual void test_merge_group2_within5_forwards()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL, P4_NORMAL, P5_NORMAL, P6_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		Schedule expected = Schedule.builder().periods(ImmutableList.of(P2_3, P4_5, P6_NORMAL)).frequency(P2M).rollConvention(DAY_17).build();
		assertEquals(test.mergeRegular(2, true), expected);
		assertEquals(test.merge(2, P2_NORMAL.UnadjustedStartDate, P5_NORMAL.UnadjustedEndDate), expected);
		assertEquals(test.merge(2, P2_NORMAL.StartDate, P5_NORMAL.EndDate), expected);
	  }

	  public virtual void test_merge_group2_within5_backwards()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL, P4_NORMAL, P5_NORMAL, P6_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		Schedule expected = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_4, P5_6)).frequency(P2M).rollConvention(DAY_17).build();
		assertEquals(test.mergeRegular(2, false), expected);
		assertEquals(test.merge(2, P3_NORMAL.UnadjustedStartDate, P6_NORMAL.UnadjustedEndDate), expected);
		assertEquals(test.merge(2, P3_NORMAL.StartDate, P6_NORMAL.EndDate), expected);
	  }

	  public virtual void test_merge_group2_within6_includeInitialStub()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P1_STUB, P2_NORMAL, P3_NORMAL, P4_NORMAL, P5_NORMAL, P6_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		Schedule expected = Schedule.builder().periods(ImmutableList.of(P1_2, P3_4, P5_6)).frequency(P2M).rollConvention(DAY_17).build();
		assertEquals(test.merge(2, P3_NORMAL.UnadjustedStartDate, P6_NORMAL.UnadjustedEndDate), expected);
		assertEquals(test.merge(2, P3_NORMAL.StartDate, P6_NORMAL.EndDate), expected);
	  }

	  public virtual void test_merge_group2_within6_includeFinalStub()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P1_STUB, P2_NORMAL, P3_NORMAL, P4_STUB)).frequency(P1M).rollConvention(DAY_17).build();
		Schedule expected = Schedule.builder().periods(ImmutableList.of(P1_2, P3_4STUB)).frequency(P2M).rollConvention(DAY_17).build();
		assertEquals(test.merge(2, P1_STUB.UnadjustedStartDate, P2_NORMAL.UnadjustedEndDate), expected);
		assertEquals(test.merge(2, P1_STUB.StartDate, P2_NORMAL.EndDate), expected);
	  }

	  public virtual void test_merge_group3_within5_forwards()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL, P4_NORMAL, P5_NORMAL, P6_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		Schedule expected = Schedule.builder().periods(ImmutableList.of(P2_4, P5_6)).frequency(P3M).rollConvention(DAY_17).build();
		assertEquals(test.mergeRegular(3, true), expected);
		assertEquals(test.merge(3, P2_NORMAL.UnadjustedStartDate, P4_NORMAL.UnadjustedEndDate), expected);
		assertEquals(test.merge(3, P2_NORMAL.StartDate, P4_NORMAL.EndDate), expected);
	  }

	  public virtual void test_merge_group3_within5_backwards()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL, P4_NORMAL, P5_NORMAL, P6_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		Schedule expected = Schedule.builder().periods(ImmutableList.of(P2_3, P4_6)).frequency(P3M).rollConvention(DAY_17).build();
		assertEquals(test.mergeRegular(3, false), expected);
		assertEquals(test.merge(3, P4_NORMAL.UnadjustedStartDate, P6_NORMAL.UnadjustedEndDate), expected);
		assertEquals(test.merge(3, P4_NORMAL.StartDate, P6_NORMAL.EndDate), expected);
	  }

	  public virtual void test_merge_termNoChange()
	  {
		Schedule test = Schedule.ofTerm(P1_STUB);
		assertEquals(test.mergeRegular(2, true), test);
		assertEquals(test.mergeRegular(2, false), test);
		assertEquals(test.merge(2, P1_STUB.UnadjustedStartDate, P1_STUB.UnadjustedEndDate), test);
		assertEquals(test.merge(2, P1_STUB.StartDate, P1_STUB.EndDate), test);
	  }

	  public virtual void test_merge_size1_stub()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P1_STUB)).frequency(P1M).rollConvention(DAY_17).build();
		assertEquals(test.mergeRegular(2, true), test);
		assertEquals(test.mergeRegular(2, false), test);
		assertEquals(test.merge(2, P1_STUB.UnadjustedStartDate, P1_STUB.UnadjustedEndDate), test);
		assertEquals(test.merge(2, P1_STUB.StartDate, P1_STUB.EndDate), test);
	  }

	  public virtual void test_merge_groupSizeOneNoChange()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL, P4_NORMAL, P5_NORMAL, P6_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		assertEquals(test.mergeRegular(1, true), test);
		assertEquals(test.mergeRegular(1, false), test);
		assertEquals(test.merge(1, P2_NORMAL.UnadjustedStartDate, P6_NORMAL.UnadjustedEndDate), test);
		assertEquals(test.merge(1, P2_NORMAL.StartDate, P6_NORMAL.EndDate), test);
	  }

	  public virtual void test_merge_groupSizeInvalid()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL, P4_NORMAL, P5_NORMAL, P6_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		assertThrowsIllegalArg(() => test.mergeRegular(0, true));
		assertThrowsIllegalArg(() => test.mergeRegular(0, false));
		assertThrowsIllegalArg(() => test.mergeRegular(-1, true));
		assertThrowsIllegalArg(() => test.mergeRegular(-1, false));
		assertThrowsIllegalArg(() => test.merge(0, P2_NORMAL.UnadjustedStartDate, P6_NORMAL.UnadjustedEndDate));
		assertThrowsIllegalArg(() => test.merge(-1, P2_NORMAL.UnadjustedStartDate, P6_NORMAL.UnadjustedEndDate));
	  }

	  public virtual void test_merge_badDate()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL, P4_NORMAL, P5_NORMAL, P6_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		assertThrows(() => test.merge(2, JUL_03, AUG_17), typeof(ScheduleException));
		assertThrows(() => test.merge(2, JUL_17, SEP_30), typeof(ScheduleException));
	  }

	  public virtual void test_merge_badGroupSize()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL, P4_NORMAL, P5_NORMAL, P6_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		assertThrows(() => test.merge(2, P2_NORMAL.UnadjustedStartDate, P6_NORMAL.UnadjustedEndDate), typeof(ScheduleException), "Unable to merge schedule, firstRegularStartDate " + P2_NORMAL.UnadjustedStartDate + " and lastRegularEndDate " + P6_NORMAL.UnadjustedEndDate + " cannot be used to create regular periods of frequency 'P2M'");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toAdjusted()
	  {
		SchedulePeriod period1 = SchedulePeriod.of(JUN_15, SEP_17);
		SchedulePeriod period2 = SchedulePeriod.of(SEP_17, SEP_30);
		Schedule test = Schedule.builder().periods(period1, period2).frequency(P3M).rollConvention(DAY_17).build();
		assertEquals(test.toAdjusted(date => date), test);
		assertEquals(test.toAdjusted(date => date.Equals(JUN_15) ? JUN_16 : date), Schedule.builder().periods(SchedulePeriod.of(JUN_16, SEP_17, JUN_15, SEP_17), period2).frequency(P3M).rollConvention(DAY_17).build());
	  }

	  public virtual void test_toUnadjusted()
	  {
		SchedulePeriod a = SchedulePeriod.of(JUL_17, OCT_17, JUL_16, OCT_15);
		SchedulePeriod b = SchedulePeriod.of(JUL_16, OCT_15, JUL_16, OCT_15);
		Schedule test = Schedule.builder().periods(ImmutableList.of(a)).frequency(P1M).rollConvention(DAY_17).build().toUnadjusted();
		Schedule expected = Schedule.builder().periods(ImmutableList.of(b)).frequency(P1M).rollConvention(DAY_17).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage_equals()
	  {
		Schedule a = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL, P4_NORMAL, P5_NORMAL, P6_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		Schedule b = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		Schedule c = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL, P4_NORMAL, P5_NORMAL, P6_NORMAL)).frequency(P3M).rollConvention(DAY_17).build();
		Schedule d = Schedule.builder().periods(ImmutableList.of(P2_NORMAL, P3_NORMAL, P4_NORMAL, P5_NORMAL, P6_NORMAL)).frequency(P1M).rollConvention(RollConventions.DAY_1).build();
		assertEquals(a.Equals(a), true);
		assertEquals(a.Equals(b), false);
		assertEquals(a.Equals(c), false);
		assertEquals(a.Equals(d), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage_builder()
	  {
		Schedule.Builder builder = Schedule.builder();
		builder.periods(P1_STUB).frequency(P1M).rollConvention(DAY_17).build();
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P1_STUB, P2_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		coverImmutableBean(test);
	  }

	  public virtual void test_serialization()
	  {
		Schedule test = Schedule.builder().periods(ImmutableList.of(P1_STUB, P2_NORMAL)).frequency(P1M).rollConvention(DAY_17).build();
		assertSerialization(test);
	  }

	}

}