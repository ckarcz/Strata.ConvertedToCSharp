/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.value
{
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
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Lists = com.google.common.collect.Lists;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="ValueSchedule"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ValueScheduleTest
	public class ValueScheduleTest
	{

	  private static double TOLERANCE = 1.0E-10;
	  private static ValueStep STEP1 = ValueStep.of(date(2014, 6, 30), ValueAdjustment.ofReplace(2000d));
	  private static ValueStep STEP2 = ValueStep.of(date(2014, 7, 30), ValueAdjustment.ofReplace(3000d));

	  private static SchedulePeriod PERIOD1 = SchedulePeriod.of(date(2014, 1, 1), date(2014, 2, 1));
	  private static SchedulePeriod PERIOD2 = SchedulePeriod.of(date(2014, 2, 1), date(2014, 3, 1));
	  private static SchedulePeriod PERIOD3 = SchedulePeriod.of(date(2014, 3, 1), date(2014, 4, 1), date(2014, 3, 2), date(2014, 4, 1));
	  private static ImmutableList<SchedulePeriod> PERIODS = ImmutableList.of(PERIOD1, PERIOD2, PERIOD3);
	  private static Schedule SCHEDULE = Schedule.builder().periods(PERIODS).frequency(Frequency.P1M).rollConvention(RollConventions.DAY_1).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_constant_ALWAYS_0()
	  {
		ValueSchedule test = ValueSchedule.ALWAYS_0;
		assertEquals(test.InitialValue, 0d, TOLERANCE);
		assertEquals(test.Steps, ImmutableList.of());
	  }

	  public virtual void test_constant_ALWAYS_1()
	  {
		ValueSchedule test = ValueSchedule.ALWAYS_1;
		assertEquals(test.InitialValue, 1d, TOLERANCE);
		assertEquals(test.Steps, ImmutableList.of());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_int()
	  {
		ValueSchedule test = ValueSchedule.of(10000d);
		assertEquals(test.InitialValue, 10000d, TOLERANCE);
		assertEquals(test.Steps, ImmutableList.of());
	  }

	  public virtual void test_of_intStepsArray()
	  {
		ValueSchedule test = ValueSchedule.of(10000d, STEP1, STEP2);
		assertEquals(test.InitialValue, 10000d, TOLERANCE);
		assertEquals(test.Steps, ImmutableList.of(STEP1, STEP2));
	  }

	  public virtual void test_of_intStepsArray_empty()
	  {
		ValueSchedule test = ValueSchedule.of(10000d, new ValueStep[0]);
		assertEquals(test.InitialValue, 10000d, TOLERANCE);
		assertEquals(test.Steps, ImmutableList.of());
	  }

	  public virtual void test_of_intStepsList()
	  {
		ValueSchedule test = ValueSchedule.of(10000d, Lists.newArrayList(STEP1, STEP2));
		assertEquals(test.InitialValue, 10000d, TOLERANCE);
		assertEquals(test.Steps, ImmutableList.of(STEP1, STEP2));
	  }

	  public virtual void test_of_intStepsList_empty()
	  {
		ValueSchedule test = ValueSchedule.of(10000d, Lists.newArrayList());
		assertEquals(test.InitialValue, 10000d, TOLERANCE);
		assertEquals(test.Steps, ImmutableList.of());
	  }

	  public virtual void test_of_sequence()
	  {
		ValueStepSequence seq = ValueStepSequence.of(date(2016, 4, 20), date(2016, 10, 20), Frequency.P3M, ValueAdjustment.ofDeltaAmount(-100));
		ValueSchedule test = ValueSchedule.of(10000d, seq);
		assertEquals(test.InitialValue, 10000d, TOLERANCE);
		assertEquals(test.Steps, ImmutableList.of());
		assertEquals(test.StepSequence, seq);
	  }

	  public virtual void test_builder_validEmpty()
	  {
		ValueSchedule test = ValueSchedule.builder().build();
		assertEquals(test.InitialValue, 0d, TOLERANCE);
		assertEquals(test.Steps, ImmutableList.of());
	  }

	  public virtual void test_builder_validFull()
	  {
		ValueStepSequence seq = ValueStepSequence.of(date(2016, 4, 20), date(2016, 10, 20), Frequency.P3M, ValueAdjustment.ofDeltaAmount(-100));
		ValueSchedule test = ValueSchedule.builder().initialValue(2000d).steps(STEP1, STEP2).stepSequence(seq).build();
		assertEquals(test.InitialValue, 2000d, TOLERANCE);
		assertEquals(test.Steps, ImmutableList.of(STEP1, STEP2));
		assertEquals(test.StepSequence, seq);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolveValues_dateBased()
	  {
		ValueStep step1 = ValueStep.of(date(2014, 2, 1), ValueAdjustment.ofReplace(300d));
		ValueStep step2 = ValueStep.of(date(2014, 3, 1), ValueAdjustment.ofReplace(400d));
		// no steps
		ValueSchedule test0 = ValueSchedule.of(200d, ImmutableList.of());
		assertEquals(test0.resolveValues(SCHEDULE), DoubleArray.of(200d, 200d, 200d));
		// step1
		ValueSchedule test1a = ValueSchedule.of(200d, ImmutableList.of(step1));
		assertEquals(test1a.resolveValues(SCHEDULE), DoubleArray.of(200d, 300d, 300d));
		// step2
		ValueSchedule test1b = ValueSchedule.of(200d, ImmutableList.of(step2));
		assertEquals(test1b.resolveValues(SCHEDULE), DoubleArray.of(200d, 200d, 400d));
		// step1 and step2
		ValueSchedule test2 = ValueSchedule.of(200d, ImmutableList.of(step1, step2));
		assertEquals(test2.resolveValues(SCHEDULE), DoubleArray.of(200d, 300d, 400d));
	  }

	  public virtual void test_resolveValues_dateBased_matchAdjusted()
	  {
		ValueStep step1 = ValueStep.of(date(2014, 2, 1), ValueAdjustment.ofReplace(300d));
		ValueStep step2 = ValueStep.of(date(2014, 3, 2), ValueAdjustment.ofReplace(400d));
		// no steps
		ValueSchedule test0 = ValueSchedule.of(200d, ImmutableList.of());
		assertEquals(test0.resolveValues(SCHEDULE), DoubleArray.of(200d, 200d, 200d));
		// step1
		ValueSchedule test1a = ValueSchedule.of(200d, ImmutableList.of(step1));
		assertEquals(test1a.resolveValues(SCHEDULE), DoubleArray.of(200d, 300d, 300d));
		// step2
		ValueSchedule test1b = ValueSchedule.of(200d, ImmutableList.of(step2));
		assertEquals(test1b.resolveValues(SCHEDULE), DoubleArray.of(200d, 200d, 400d));
		// step1 and step2
		ValueSchedule test2 = ValueSchedule.of(200d, ImmutableList.of(step1, step2));
		assertEquals(test2.resolveValues(SCHEDULE), DoubleArray.of(200d, 300d, 400d));
	  }

	  public virtual void test_resolveValues_dateBased_ignoreExcess()
	  {
		ValueStep step1 = ValueStep.of(date(2014, 2, 1), ValueAdjustment.ofReplace(300d));
		ValueStep step2 = ValueStep.of(date(2014, 2, 15), ValueAdjustment.ofReplace(300d)); // no change to value
		ValueStep step3 = ValueStep.of(date(2014, 3, 1), ValueAdjustment.ofReplace(400d));
		ValueStep step4 = ValueStep.of(date(2014, 3, 15), ValueAdjustment.ofDeltaAmount(0d)); // no change to value
		ValueStep step5 = ValueStep.of(date(2014, 4, 1), ValueAdjustment.ofMultiplier(1d));
		ValueSchedule test = ValueSchedule.of(200d, ImmutableList.of(step1, step2, step3, step4, step5));
		assertEquals(test.resolveValues(SCHEDULE), DoubleArray.of(200d, 300d, 400d));
	  }

	  public virtual void test_resolveValues_indexBased()
	  {
		ValueStep step1 = ValueStep.of(1, ValueAdjustment.ofReplace(300d));
		ValueStep step2 = ValueStep.of(2, ValueAdjustment.ofReplace(400d));
		// no steps
		ValueSchedule test0 = ValueSchedule.of(200d, ImmutableList.of());
		assertEquals(test0.resolveValues(SCHEDULE), DoubleArray.of(200d, 200d, 200d));
		// step1
		ValueSchedule test1a = ValueSchedule.of(200d, ImmutableList.of(step1));
		assertEquals(test1a.resolveValues(SCHEDULE), DoubleArray.of(200d, 300d, 300d));
		// step2
		ValueSchedule test1b = ValueSchedule.of(200d, ImmutableList.of(step2));
		assertEquals(test1b.resolveValues(SCHEDULE), DoubleArray.of(200d, 200d, 400d));
		// step1 and step2
		ValueSchedule test2 = ValueSchedule.of(200d, ImmutableList.of(step1, step2));
		assertEquals(test2.resolveValues(SCHEDULE), DoubleArray.of(200d, 300d, 400d));
	  }

	  public virtual void test_resolveValues_indexBased_duplicateDefinitionValid()
	  {
		ValueStep step1 = ValueStep.of(1, ValueAdjustment.ofReplace(300d));
		ValueStep step2 = ValueStep.of(1, ValueAdjustment.ofReplace(300d));
		ValueSchedule test = ValueSchedule.of(200d, ImmutableList.of(step1, step2));
		assertEquals(test.resolveValues(SCHEDULE), DoubleArray.of(200d, 300d, 300d));
	  }

	  public virtual void test_resolveValues_indexBased_duplicateDefinitionInvalid()
	  {
		ValueStep step1 = ValueStep.of(1, ValueAdjustment.ofReplace(300d));
		ValueStep step2 = ValueStep.of(1, ValueAdjustment.ofReplace(400d));
		ValueSchedule test = ValueSchedule.of(200d, ImmutableList.of(step1, step2));
		assertThrowsIllegalArg(() => test.resolveValues(SCHEDULE));
	  }

	  public virtual void test_resolveValues_dateBased_indexZeroValid()
	  {
		ValueStep step = ValueStep.of(date(2014, 1, 1), ValueAdjustment.ofReplace(300d));
		ValueSchedule test = ValueSchedule.of(200d, ImmutableList.of(step));
		assertEquals(test.resolveValues(SCHEDULE), DoubleArray.of(300d, 300d, 300d));
	  }

	  public virtual void test_resolveValues_indexBased_indexTooBig()
	  {
		ValueStep step = ValueStep.of(3, ValueAdjustment.ofReplace(300d));
		ValueSchedule test = ValueSchedule.of(200d, ImmutableList.of(step));
		assertThrowsIllegalArg(() => test.resolveValues(SCHEDULE));
	  }

	  public virtual void test_resolveValues_dateBased_invalidChangeValue()
	  {
		ValueStep step = ValueStep.of(date(2014, 4, 1), ValueAdjustment.ofReplace(300d));
		ValueSchedule test = ValueSchedule.of(200d, ImmutableList.of(step));
		assertThrowsIllegalArg(() => test.resolveValues(SCHEDULE), "ValueStep date does not match a period boundary.*");
	  }

	  public virtual void test_resolveValues_dateBased_invalidDateBefore()
	  {
		ValueStep step = ValueStep.of(date(2013, 12, 31), ValueAdjustment.ofReplace(300d));
		ValueSchedule test = ValueSchedule.of(200d, ImmutableList.of(step));
		assertThrowsIllegalArg(() => test.resolveValues(SCHEDULE), "ValueStep date is before the start of the schedule.*");
	  }

	  public virtual void test_resolveValues_dateBased_invalidDateAfter()
	  {
		ValueStep step = ValueStep.of(date(2014, 4, 3), ValueAdjustment.ofReplace(300d));
		ValueSchedule test = ValueSchedule.of(200d, ImmutableList.of(step));
		assertThrowsIllegalArg(() => test.resolveValues(SCHEDULE), "ValueStep date is after the end of the schedule.*");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolveValues_sequence()
	  {
		ValueStepSequence seq = ValueStepSequence.of(date(2014, 2, 1), date(2014, 3, 1), Frequency.P1M, ValueAdjustment.ofDeltaAmount(100));
		ValueSchedule test = ValueSchedule.of(200d, seq);
		assertEquals(test.resolveValues(SCHEDULE), DoubleArray.of(200d, 300d, 400d));
	  }

	  public virtual void test_resolveValues_sequenceAndSteps()
	  {
		ValueStepSequence seq = ValueStepSequence.of(date(2014, 2, 1), date(2014, 3, 1), Frequency.P1M, ValueAdjustment.ofDeltaAmount(100));
		ValueStep step1 = ValueStep.of(date(2014, 1, 1), ValueAdjustment.ofReplace(350d));
		ValueSchedule test = ValueSchedule.builder().initialValue(200d).steps(step1).stepSequence(seq).build();
		assertEquals(test.resolveValues(SCHEDULE), DoubleArray.of(350d, 450d, 550d));
	  }

	  public virtual void test_resolveValues_sequenceAndStepClash()
	  {
		ValueStepSequence seq = ValueStepSequence.of(date(2014, 2, 1), date(2014, 3, 1), Frequency.P1M, ValueAdjustment.ofDeltaAmount(100));
		ValueStep step1 = ValueStep.of(date(2014, 2, 1), ValueAdjustment.ofReplace(350d));
		ValueSchedule test = ValueSchedule.builder().initialValue(200d).steps(step1).stepSequence(seq).build();
		assertThrowsIllegalArg(() => test.resolveValues(SCHEDULE));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void Equals()
	  {
		ValueSchedule a1 = ValueSchedule.of(10000d, Lists.newArrayList(STEP1, STEP2));
		ValueSchedule a2 = ValueSchedule.of(10000d, Lists.newArrayList(STEP1, STEP2));
		ValueSchedule b = ValueSchedule.of(5000d, Lists.newArrayList(STEP1, STEP2));
		ValueSchedule c = ValueSchedule.of(10000d, Lists.newArrayList(STEP1));
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(c), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ValueStepSequence seq = ValueStepSequence.of(date(2014, 2, 1), date(2014, 3, 1), Frequency.P1M, ValueAdjustment.ofDeltaAmount(100));

		ValueSchedule test = ValueSchedule.of(10000d, Lists.newArrayList(STEP1, STEP2));
		coverImmutableBean(test);
		coverBeanEquals(test, ValueSchedule.of(20000d, seq));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(ValueSchedule.of(10000d, Lists.newArrayList(STEP1, STEP2)));
	  }

	}

}