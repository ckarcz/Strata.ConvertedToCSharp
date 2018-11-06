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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_1W;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_2M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.RollConventions.DAY_5;
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
//	import static com.opengamma.strata.product.swap.FixingRelativeTo.PERIOD_END;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.FixingRelativeTo.PERIOD_START;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.IborRateResetMethod.UNWEIGHTED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.IborRateResetMethod.WEIGHTED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.NegativeRateMethod.ALLOW_NEGATIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.NegativeRateMethod.NOT_NEGATIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using Index = com.opengamma.strata.basics.index.Index;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using ValueAdjustment = com.opengamma.strata.basics.value.ValueAdjustment;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ValueStep = com.opengamma.strata.basics.value.ValueStep;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using IborAveragedFixing = com.opengamma.strata.product.rate.IborAveragedFixing;
	using IborAveragedRateComputation = com.opengamma.strata.product.rate.IborAveragedRateComputation;
	using IborInterpolatedRateComputation = com.opengamma.strata.product.rate.IborInterpolatedRateComputation;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborRateCalculationTest
	public class IborRateCalculationTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE_01_02 = date(2014, 1, 2);
	  private static readonly LocalDate DATE_01_03 = date(2014, 1, 3);
	  private static readonly LocalDate DATE_01_05 = date(2014, 1, 5);
	  private static readonly LocalDate DATE_01_06 = date(2014, 1, 6);
	  private static readonly LocalDate DATE_01_07 = date(2014, 1, 7);
	  private static readonly LocalDate DATE_01_08 = date(2014, 1, 8);
	  private static readonly LocalDate DATE_01_31 = date(2014, 1, 31);
	  private static readonly LocalDate DATE_02_03 = date(2014, 2, 3);
	  private static readonly LocalDate DATE_02_05 = date(2014, 2, 5);
	  private static readonly LocalDate DATE_02_28 = date(2014, 2, 28);
	  private static readonly LocalDate DATE_03_03 = date(2014, 3, 3);
	  private static readonly LocalDate DATE_03_05 = date(2014, 3, 5);
	  private static readonly LocalDate DATE_04_02 = date(2014, 4, 2);
	  private static readonly LocalDate DATE_04_03 = date(2014, 4, 3);
	  private static readonly LocalDate DATE_04_04 = date(2014, 4, 4);
	  private static readonly LocalDate DATE_04_05 = date(2014, 4, 5);
	  private static readonly LocalDate DATE_04_07 = date(2014, 4, 7);
	  private static readonly LocalDate DATE_05_01 = date(2014, 5, 1);
	  private static readonly LocalDate DATE_05_06 = date(2014, 5, 6);
	  private static readonly LocalDate DATE_06_03 = date(2014, 6, 3);
	  private static readonly LocalDate DATE_06_05 = date(2014, 6, 5);
	  private static readonly LocalDate DATE_07_05 = date(2014, 7, 5);
	  private static readonly LocalDate DATE_07_07 = date(2014, 7, 7);
	  private static readonly DaysAdjustment MINUS_ONE_DAY = DaysAdjustment.ofBusinessDays(-1, GBLO);
	  private static readonly DaysAdjustment MINUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(-2, GBLO);
	  private static readonly DaysAdjustment MINUS_THREE_DAYS = DaysAdjustment.ofBusinessDays(-3, GBLO);

	  private static readonly SchedulePeriod ACCRUAL1STUB = SchedulePeriod.of(DATE_01_08, DATE_02_05, DATE_01_08, DATE_02_05);
	  private static readonly SchedulePeriod ACCRUAL1 = SchedulePeriod.of(DATE_01_06, DATE_02_05, DATE_01_05, DATE_02_05);
	  private static readonly SchedulePeriod ACCRUAL2 = SchedulePeriod.of(DATE_02_05, DATE_03_05, DATE_02_05, DATE_03_05);
	  private static readonly SchedulePeriod ACCRUAL3 = SchedulePeriod.of(DATE_03_05, DATE_04_07, DATE_03_05, DATE_04_05);
	  private static readonly SchedulePeriod ACCRUAL3STUB = SchedulePeriod.of(DATE_03_05, DATE_04_04, DATE_03_05, DATE_04_04);
	  private static readonly Schedule ACCRUAL_SCHEDULE = Schedule.builder().periods(ACCRUAL1, ACCRUAL2, ACCRUAL3).frequency(P1M).rollConvention(DAY_5).build();
	  private static readonly Schedule ACCRUAL_SCHEDULE_STUBS = Schedule.builder().periods(ACCRUAL1STUB, ACCRUAL2, ACCRUAL3STUB).frequency(P1M).rollConvention(DAY_5).build();
	  private static readonly Schedule ACCRUAL_SCHEDULE_INITIAL_STUB = Schedule.builder().periods(ACCRUAL1STUB, ACCRUAL2, ACCRUAL3).frequency(P1M).rollConvention(DAY_5).build();
	  private static readonly Schedule ACCRUAL_SCHEDULE_FINAL_STUB = Schedule.builder().periods(ACCRUAL1, ACCRUAL2, ACCRUAL3STUB).frequency(P1M).rollConvention(DAY_5).build();
	  private static readonly Schedule SINGLE_ACCRUAL_SCHEDULE_STUB = Schedule.builder().periods(ACCRUAL1STUB).frequency(P3M).rollConvention(DAY_5).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		IborRateCalculation test = IborRateCalculation.of(GBP_LIBOR_3M);
		assertEquals(test.Type, SwapLegType.IBOR);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.ResetPeriods, null);
		assertEquals(test.FixingRelativeTo, PERIOD_START);
		assertEquals(test.FixingDateOffset, GBP_LIBOR_3M.FixingDateOffset);
		assertEquals(test.NegativeRateMethod, ALLOW_NEGATIVE);
		assertEquals(test.FirstRegularRate, double?.empty());
		assertEquals(test.InitialStub, null);
		assertEquals(test.FinalStub, null);
		assertEquals(test.Gearing, null);
		assertEquals(test.Spread, null);
	  }

	  public virtual void test_builder_ensureDefaults()
	  {
		IborRateCalculation test = IborRateCalculation.builder().index(GBP_LIBOR_3M).build();
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.ResetPeriods, null);
		assertEquals(test.FixingRelativeTo, PERIOD_START);
		assertEquals(test.FixingDateOffset, GBP_LIBOR_3M.FixingDateOffset);
		assertEquals(test.NegativeRateMethod, ALLOW_NEGATIVE);
		assertEquals(test.FirstRegularRate, double?.empty());
		assertEquals(test.InitialStub, null);
		assertEquals(test.FinalStub, null);
		assertEquals(test.Gearing, null);
		assertEquals(test.Spread, null);
	  }

	  public virtual void test_builder_ensureOptionalDouble()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_3M).fixingDateOffset(MINUS_TWO_DAYS).firstRegularRate(0.028d).build();
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.ResetPeriods, null);
		assertEquals(test.FixingRelativeTo, PERIOD_START);
		assertEquals(test.FixingDateOffset, MINUS_TWO_DAYS);
		assertEquals(test.NegativeRateMethod, ALLOW_NEGATIVE);
		assertEquals(test.FirstRegularRate, double?.of(0.028d));
		assertEquals(test.InitialStub, null);
		assertEquals(test.FinalStub, null);
		assertEquals(test.Gearing, null);
		assertEquals(test.Spread, null);
	  }

	  public virtual void test_builder_noIndex()
	  {
		assertThrowsIllegalArg(() => IborRateCalculation.builder().build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices_simple()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).build();
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GBP_LIBOR_1M));
	  }

	  public virtual void test_collectIndices_stubCalcsTwoStubs()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).initialStub(IborRateStubCalculation.ofIborRate(GBP_LIBOR_1W)).finalStub(IborRateStubCalculation.ofIborRate(GBP_LIBOR_3M)).build();
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GBP_LIBOR_1M, GBP_LIBOR_1W, GBP_LIBOR_3M));
	  }

	  public virtual void test_collectIndices_stubCalcsTwoStubs_interpolated()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).initialStub(IborRateStubCalculation.ofIborInterpolatedRate(GBP_LIBOR_1W, GBP_LIBOR_1M)).finalStub(IborRateStubCalculation.ofIborInterpolatedRate(GBP_LIBOR_3M, GBP_LIBOR_1M)).build();
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GBP_LIBOR_1M, GBP_LIBOR_1W, GBP_LIBOR_3M));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_expand_simple()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1).yearFraction(ACCRUAL1.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_01_02, REF_DATA)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_02_03, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_03_03, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE, ACCRUAL_SCHEDULE, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_simpleFinalStub()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1).yearFraction(ACCRUAL1.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_FINAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_01_02, REF_DATA)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_FINAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_02_03, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3STUB).yearFraction(ACCRUAL3STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_FINAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_03_03, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE_FINAL_STUB, ACCRUAL_SCHEDULE_FINAL_STUB, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_simpleInitialStub()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1STUB).yearFraction(ACCRUAL1STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_01_06, REF_DATA)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_02_03, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_03_03, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE_INITIAL_STUB, ACCRUAL_SCHEDULE_INITIAL_STUB, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_simpleTwoStubs()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1STUB).yearFraction(ACCRUAL1STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_STUBS)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_01_06, REF_DATA)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_STUBS)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_02_03, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3STUB).yearFraction(ACCRUAL3STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_STUBS)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_03_03, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE_STUBS, ACCRUAL_SCHEDULE_STUBS, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_expand_stubCalcsTwoStubs()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).initialStub(IborRateStubCalculation.ofIborRate(GBP_LIBOR_1W)).finalStub(IborRateStubCalculation.ofIborRate(GBP_LIBOR_3M)).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1STUB).yearFraction(ACCRUAL1STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_STUBS)).rateComputation(IborRateComputation.of(GBP_LIBOR_1W, DATE_01_06, REF_DATA)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_STUBS)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_02_03, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3STUB).yearFraction(ACCRUAL3STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_STUBS)).rateComputation(IborRateComputation.of(GBP_LIBOR_3M, DATE_03_03, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE_STUBS, ACCRUAL_SCHEDULE_STUBS, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_stubCalcsTwoStubs_interpolated()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).initialStub(IborRateStubCalculation.ofIborInterpolatedRate(GBP_LIBOR_1W, GBP_LIBOR_1M)).finalStub(IborRateStubCalculation.ofIborInterpolatedRate(GBP_LIBOR_3M, GBP_LIBOR_1M)).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1STUB).yearFraction(ACCRUAL1STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_STUBS)).rateComputation(IborInterpolatedRateComputation.of(GBP_LIBOR_1W, GBP_LIBOR_1M, DATE_01_06, REF_DATA)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_STUBS)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_02_03, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3STUB).yearFraction(ACCRUAL3STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_STUBS)).rateComputation(IborInterpolatedRateComputation.of(GBP_LIBOR_1M, GBP_LIBOR_3M, DATE_03_03, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE_STUBS, ACCRUAL_SCHEDULE_STUBS, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_singlePeriod_stubCalcsInitialStub_interpolated()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_2M).fixingDateOffset(MINUS_TWO_DAYS).initialStub(IborRateStubCalculation.ofIborInterpolatedRate(GBP_LIBOR_1W, GBP_LIBOR_1M)).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1STUB).yearFraction(ACCRUAL1STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_STUBS)).rateComputation(IborInterpolatedRateComputation.of(GBP_LIBOR_1W, GBP_LIBOR_1M, DATE_01_06, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(SINGLE_ACCRUAL_SCHEDULE_STUB, SINGLE_ACCRUAL_SCHEDULE_STUB, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_expand_firstFixingDateOffsetNoStub()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).firstFixingDateOffset(MINUS_ONE_DAY).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1).yearFraction(ACCRUAL1.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_01_03, REF_DATA)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_02_03, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_03_03, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE, ACCRUAL_SCHEDULE, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_firstFixingDateOffsetInitialStub()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).firstFixingDateOffset(MINUS_ONE_DAY).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1STUB).yearFraction(ACCRUAL1STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_01_07, REF_DATA)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_02_03, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_03_03, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE_INITIAL_STUB, ACCRUAL_SCHEDULE_INITIAL_STUB, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_expand_firstRegularRateFixed()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).firstRegularRate(0.028d).firstRate(0.024d).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1).yearFraction(ACCRUAL1.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(FixedRateComputation.of(0.028d)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_02_03, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_03_03, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE, ACCRUAL_SCHEDULE, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_firstRegularRateFixedInitialStub()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).firstRegularRate(0.028d).firstRate(0.024d).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1STUB).yearFraction(ACCRUAL1STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_01_06, REF_DATA)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(FixedRateComputation.of(0.028d)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_03_03, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE_INITIAL_STUB, ACCRUAL_SCHEDULE_INITIAL_STUB, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_firstRegularRateFixedTwoStubs()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).firstRegularRate(0.028d).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1STUB).yearFraction(ACCRUAL1STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_STUBS)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_01_06, REF_DATA)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_STUBS)).rateComputation(FixedRateComputation.of(0.028d)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3STUB).yearFraction(ACCRUAL3STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_STUBS)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_03_03, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE_STUBS, ACCRUAL_SCHEDULE_STUBS, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_expand_firstRateFixed()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).firstRate(0.024d).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1).yearFraction(ACCRUAL1.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(FixedRateComputation.of(0.024d)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_02_03, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_365F, ACCRUAL_SCHEDULE)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_03_03, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE, ACCRUAL_SCHEDULE, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_firstRateFixedInitialStubNotSpecified()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).firstRate(0.024d).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1STUB).yearFraction(ACCRUAL1STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(FixedRateComputation.of(0.024d)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_02_03, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_03_03, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE_INITIAL_STUB, ACCRUAL_SCHEDULE_INITIAL_STUB, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_firstRateFixedInitialStubSpecifiedNone()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).initialStub(IborRateStubCalculation.NONE).firstRate(0.024d).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1STUB).yearFraction(ACCRUAL1STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(FixedRateComputation.of(0.024d)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_02_03, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_03_03, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE_INITIAL_STUB, ACCRUAL_SCHEDULE_INITIAL_STUB, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  public virtual void test_expand_firstRateFixedInitialStubSpecified()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_1M).fixingDateOffset(MINUS_TWO_DAYS).initialStub(IborRateStubCalculation.ofIborRate(GBP_LIBOR_1W)).firstRate(0.024d).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1STUB).yearFraction(ACCRUAL1STUB.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1W, DATE_01_06, REF_DATA)).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_02_03, REF_DATA)).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_365F, ACCRUAL_SCHEDULE_INITIAL_STUB)).rateComputation(IborRateComputation.of(GBP_LIBOR_1M, DATE_03_03, REF_DATA)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE_INITIAL_STUB, ACCRUAL_SCHEDULE_INITIAL_STUB, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_expand_resetPeriods_weighted()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_3M).fixingDateOffset(MINUS_TWO_DAYS).resetPeriods(ResetSchedule.builder().resetFrequency(P1M).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, GBLO)).resetMethod(WEIGHTED).build()).build();

		SchedulePeriod accrual1 = SchedulePeriod.of(DATE_01_06, DATE_04_07, DATE_01_05, DATE_04_05);
		SchedulePeriod accrual2 = SchedulePeriod.of(DATE_04_07, DATE_07_07, DATE_04_05, DATE_07_05);
		Schedule schedule = Schedule.builder().periods(accrual1, accrual2).frequency(P3M).rollConvention(DAY_5).build();

		IborIndexObservation obs1 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_01_02, REF_DATA);
		IborIndexObservation obs2 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_02_03, REF_DATA);
		IborIndexObservation obs3 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_03_03, REF_DATA);
		ImmutableList<IborAveragedFixing> fixings1 = ImmutableList.of(IborAveragedFixing.ofDaysInResetPeriod(obs1, DATE_01_06, DATE_02_05), IborAveragedFixing.ofDaysInResetPeriod(obs2, DATE_02_05, DATE_03_05), IborAveragedFixing.ofDaysInResetPeriod(obs3, DATE_03_05, DATE_04_07));
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(accrual1).yearFraction(accrual1.yearFraction(ACT_365F, schedule)).rateComputation(IborAveragedRateComputation.of(fixings1)).build();

		IborIndexObservation obs4 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_04_03, REF_DATA);
		IborIndexObservation obs5 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_05_01, REF_DATA);
		IborIndexObservation obs6 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_06_03, REF_DATA);
		ImmutableList<IborAveragedFixing> fixings2 = ImmutableList.of(IborAveragedFixing.ofDaysInResetPeriod(obs4, DATE_04_07, DATE_05_06), IborAveragedFixing.ofDaysInResetPeriod(obs5, DATE_05_06, DATE_06_05), IborAveragedFixing.ofDaysInResetPeriod(obs6, DATE_06_05, DATE_07_07));
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(accrual2).yearFraction(accrual2.yearFraction(ACT_365F, schedule)).rateComputation(IborAveragedRateComputation.of(fixings2)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(schedule, schedule, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2));
	  }

	  public virtual void test_expand_resetPeriods_weighted_firstFixed()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_3M).fixingDateOffset(MINUS_TWO_DAYS).resetPeriods(ResetSchedule.builder().resetFrequency(P1M).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, GBLO)).resetMethod(WEIGHTED).build()).firstRegularRate(0.028d).build();

		SchedulePeriod accrual1 = SchedulePeriod.of(DATE_01_06, DATE_04_07, DATE_01_05, DATE_04_05);
		SchedulePeriod accrual2 = SchedulePeriod.of(DATE_04_07, DATE_07_07, DATE_04_05, DATE_07_05);
		Schedule schedule = Schedule.builder().periods(accrual1, accrual2).frequency(P3M).rollConvention(DAY_5).build();

		IborIndexObservation obs1 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_01_02, REF_DATA);
		IborIndexObservation obs2 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_02_03, REF_DATA);
		IborIndexObservation obs3 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_03_03, REF_DATA);
		ImmutableList<IborAveragedFixing> fixings1 = ImmutableList.of(IborAveragedFixing.ofDaysInResetPeriod(obs1, DATE_01_06, DATE_02_05, 0.028d), IborAveragedFixing.ofDaysInResetPeriod(obs2, DATE_02_05, DATE_03_05), IborAveragedFixing.ofDaysInResetPeriod(obs3, DATE_03_05, DATE_04_07));
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(accrual1).yearFraction(accrual1.yearFraction(ACT_365F, schedule)).rateComputation(IborAveragedRateComputation.of(fixings1)).build();

		IborIndexObservation obs4 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_04_03, REF_DATA);
		IborIndexObservation obs5 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_05_01, REF_DATA);
		IborIndexObservation obs6 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_06_03, REF_DATA);
		ImmutableList<IborAveragedFixing> fixings2 = ImmutableList.of(IborAveragedFixing.ofDaysInResetPeriod(obs4, DATE_04_07, DATE_05_06), IborAveragedFixing.ofDaysInResetPeriod(obs5, DATE_05_06, DATE_06_05), IborAveragedFixing.ofDaysInResetPeriod(obs6, DATE_06_05, DATE_07_07));
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(accrual2).yearFraction(accrual2.yearFraction(ACT_365F, schedule)).rateComputation(IborAveragedRateComputation.of(fixings2)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(schedule, schedule, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2));
	  }

	  public virtual void test_expand_resetPeriods_weighted_firstFixingDateOffset()
	  {
		// only the fixing date of the first reset period is changed, everything else stays the same
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_3M).fixingDateOffset(MINUS_TWO_DAYS).resetPeriods(ResetSchedule.builder().resetFrequency(P1M).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, GBLO)).resetMethod(WEIGHTED).build()).firstFixingDateOffset(MINUS_ONE_DAY).build();

		SchedulePeriod accrual1 = SchedulePeriod.of(DATE_01_06, DATE_04_07, DATE_01_05, DATE_04_05);
		SchedulePeriod accrual2 = SchedulePeriod.of(DATE_04_07, DATE_07_07, DATE_04_05, DATE_07_05);
		Schedule schedule = Schedule.builder().periods(accrual1, accrual2).frequency(P3M).rollConvention(DAY_5).build();

		IborIndexObservation obs1 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_01_03, REF_DATA);
		IborIndexObservation obs2 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_02_03, REF_DATA);
		IborIndexObservation obs3 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_03_03, REF_DATA);
		ImmutableList<IborAveragedFixing> fixings1 = ImmutableList.of(IborAveragedFixing.ofDaysInResetPeriod(obs1, DATE_01_06, DATE_02_05), IborAveragedFixing.ofDaysInResetPeriod(obs2, DATE_02_05, DATE_03_05), IborAveragedFixing.ofDaysInResetPeriod(obs3, DATE_03_05, DATE_04_07));
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(accrual1).yearFraction(accrual1.yearFraction(ACT_365F, schedule)).rateComputation(IborAveragedRateComputation.of(fixings1)).build();

		IborIndexObservation obs4 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_04_03, REF_DATA);
		IborIndexObservation obs5 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_05_01, REF_DATA);
		IborIndexObservation obs6 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_06_03, REF_DATA);
		ImmutableList<IborAveragedFixing> fixings2 = ImmutableList.of(IborAveragedFixing.ofDaysInResetPeriod(obs4, DATE_04_07, DATE_05_06), IborAveragedFixing.ofDaysInResetPeriod(obs5, DATE_05_06, DATE_06_05), IborAveragedFixing.ofDaysInResetPeriod(obs6, DATE_06_05, DATE_07_07));
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(accrual2).yearFraction(accrual2.yearFraction(ACT_365F, schedule)).rateComputation(IborAveragedRateComputation.of(fixings2)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(schedule, schedule, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2));
	  }

	  public virtual void test_expand_resetPeriods_unweighted()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_3M).fixingDateOffset(MINUS_TWO_DAYS).resetPeriods(ResetSchedule.builder().resetFrequency(P1M).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, GBLO)).resetMethod(UNWEIGHTED).build()).build();

		SchedulePeriod accrual1 = SchedulePeriod.of(DATE_01_06, DATE_04_07, DATE_01_05, DATE_04_05);
		SchedulePeriod accrual2 = SchedulePeriod.of(DATE_04_07, DATE_07_07, DATE_04_05, DATE_07_05);
		Schedule schedule = Schedule.builder().periods(accrual1, accrual2).frequency(P3M).rollConvention(DAY_5).build();

		IborIndexObservation obs1 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_01_02, REF_DATA);
		IborIndexObservation obs2 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_02_03, REF_DATA);
		IborIndexObservation obs3 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_03_03, REF_DATA);
		ImmutableList<IborAveragedFixing> fixings1 = ImmutableList.of(IborAveragedFixing.of(obs1), IborAveragedFixing.of(obs2), IborAveragedFixing.of(obs3));
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(accrual1).yearFraction(accrual1.yearFraction(ACT_365F, schedule)).rateComputation(IborAveragedRateComputation.of(fixings1)).build();

		IborIndexObservation obs4 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_04_03, REF_DATA);
		IborIndexObservation obs5 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_05_01, REF_DATA);
		IborIndexObservation obs6 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_06_03, REF_DATA);
		ImmutableList<IborAveragedFixing> fixings2 = ImmutableList.of(IborAveragedFixing.of(obs4), IborAveragedFixing.of(obs5), IborAveragedFixing.of(obs6));
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(accrual2).yearFraction(accrual2.yearFraction(ACT_365F, schedule)).rateComputation(IborAveragedRateComputation.of(fixings2)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(schedule, schedule, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2));
	  }

	  public virtual void test_expand_initialStubAndResetPeriods_weighted_firstFixed()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_360).index(GBP_LIBOR_3M).fixingDateOffset(MINUS_TWO_DAYS).resetPeriods(ResetSchedule.builder().resetFrequency(P1M).businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, GBLO)).resetMethod(WEIGHTED).build()).firstRegularRate(0.028d).initialStub(IborRateStubCalculation.ofFixedRate(0.030d)).build();

		SchedulePeriod accrual1 = SchedulePeriod.of(DATE_02_05, DATE_04_07, DATE_02_05, DATE_04_05);
		SchedulePeriod accrual2 = SchedulePeriod.of(DATE_04_07, DATE_07_07, DATE_04_05, DATE_07_05);
		Schedule schedule = Schedule.builder().periods(accrual1, accrual2).frequency(P3M).rollConvention(DAY_5).build();

		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(accrual1).yearFraction(accrual1.yearFraction(ACT_360, schedule)).rateComputation(FixedRateComputation.of(0.030d)).build();
		IborIndexObservation obs4 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_04_03, REF_DATA);
		IborIndexObservation obs5 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_05_01, REF_DATA);
		IborIndexObservation obs6 = IborIndexObservation.of(GBP_LIBOR_3M, DATE_06_03, REF_DATA);
		ImmutableList<IborAveragedFixing> fixings2 = ImmutableList.of(IborAveragedFixing.ofDaysInResetPeriod(obs4, DATE_04_07, DATE_05_06, 0.028d), IborAveragedFixing.ofDaysInResetPeriod(obs5, DATE_05_06, DATE_06_05), IborAveragedFixing.ofDaysInResetPeriod(obs6, DATE_06_05, DATE_07_07));
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(accrual2).yearFraction(accrual2.yearFraction(ACT_360, schedule)).rateComputation(IborAveragedRateComputation.of(fixings2)).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(schedule, schedule, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_expand_gearingSpreadEverythingElse()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_360).index(GBP_LIBOR_3M).fixingDateOffset(MINUS_THREE_DAYS).fixingRelativeTo(PERIOD_END).negativeRateMethod(NOT_NEGATIVE).gearing(ValueSchedule.of(1d, ValueStep.of(2, ValueAdjustment.ofReplace(2d)))).spread(ValueSchedule.of(0d, ValueStep.of(1, ValueAdjustment.ofReplace(-0.025d)))).build();
		RateAccrualPeriod rap1 = RateAccrualPeriod.builder(ACCRUAL1).yearFraction(ACCRUAL1.yearFraction(ACT_360, ACCRUAL_SCHEDULE)).rateComputation(IborRateComputation.of(GBP_LIBOR_3M, DATE_01_31, REF_DATA)).negativeRateMethod(NOT_NEGATIVE).build();
		RateAccrualPeriod rap2 = RateAccrualPeriod.builder(ACCRUAL2).yearFraction(ACCRUAL2.yearFraction(ACT_360, ACCRUAL_SCHEDULE)).rateComputation(IborRateComputation.of(GBP_LIBOR_3M, DATE_02_28, REF_DATA)).negativeRateMethod(NOT_NEGATIVE).spread(-0.025d).build();
		RateAccrualPeriod rap3 = RateAccrualPeriod.builder(ACCRUAL3).yearFraction(ACCRUAL3.yearFraction(ACT_360, ACCRUAL_SCHEDULE)).rateComputation(IborRateComputation.of(GBP_LIBOR_3M, DATE_04_02, REF_DATA)).negativeRateMethod(NOT_NEGATIVE).gearing(2d).spread(-0.025d).build();
		ImmutableList<RateAccrualPeriod> periods = test.createAccrualPeriods(ACCRUAL_SCHEDULE, ACCRUAL_SCHEDULE, REF_DATA);
		assertEquals(periods, ImmutableList.of(rap1, rap2, rap3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_3M).fixingDateOffset(MINUS_TWO_DAYS).build();
		coverImmutableBean(test);
		IborRateCalculation test2 = IborRateCalculation.builder().dayCount(ACT_360).index(GBP_LIBOR_6M).resetPeriods(ResetSchedule.builder().resetFrequency(P3M).resetMethod(IborRateResetMethod.UNWEIGHTED).businessDayAdjustment(BusinessDayAdjustment.NONE).build()).fixingDateOffset(MINUS_THREE_DAYS).fixingRelativeTo(PERIOD_END).negativeRateMethod(NOT_NEGATIVE).firstRegularRate(0.028d).initialStub(IborRateStubCalculation.NONE).finalStub(IborRateStubCalculation.NONE).gearing(ValueSchedule.of(2d)).spread(ValueSchedule.of(-0.025d)).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborRateCalculation test = IborRateCalculation.builder().dayCount(ACT_365F).index(GBP_LIBOR_3M).fixingDateOffset(MINUS_TWO_DAYS).build();
		assertSerialization(test);
	  }

	}

}