using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;

	/// <summary>
	/// Test <seealso cref="ValueStepSequence"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ValueStepSequenceTest
	public class ValueStepSequenceTest
	{

	  private static readonly ValueAdjustment ADJ = ValueAdjustment.ofDeltaAmount(-100);
	  private static readonly ValueAdjustment ADJ2 = ValueAdjustment.ofDeltaAmount(-200);
	  private static readonly ValueAdjustment ADJ_BAD = ValueAdjustment.ofReplace(100);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ValueStepSequence test = ValueStepSequence.of(date(2016, 4, 20), date(2016, 10, 20), Frequency.P3M, ADJ);
		assertEquals(test.FirstStepDate, date(2016, 4, 20));
		assertEquals(test.LastStepDate, date(2016, 10, 20));
		assertEquals(test.Frequency, Frequency.P3M);
		assertEquals(test.Adjustment, ADJ);
	  }

	  public virtual void test_of_invalid()
	  {
		assertThrowsIllegalArg(() => ValueStepSequence.of(date(2016, 4, 20), date(2016, 4, 19), Frequency.P3M, ADJ));
		assertThrowsIllegalArg(() => ValueStepSequence.of(date(2016, 4, 20), date(2016, 10, 20), Frequency.P3M, ADJ_BAD));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		ValueStepSequence test = ValueStepSequence.of(date(2016, 4, 20), date(2016, 10, 20), Frequency.P3M, ADJ);
		ValueStep baseStep = ValueStep.of(date(2016, 1, 20), ValueAdjustment.ofReplace(500d));
		IList<ValueStep> steps = test.resolve(ImmutableList.of(baseStep), RollConventions.NONE);
		assertEquals(steps.Count, 4);
		assertEquals(steps[0], baseStep);
		assertEquals(steps[1], ValueStep.of(date(2016, 4, 20), ADJ));
		assertEquals(steps[2], ValueStep.of(date(2016, 7, 20), ADJ));
		assertEquals(steps[3], ValueStep.of(date(2016, 10, 20), ADJ));
	  }

	  public virtual void test_resolve_invalid()
	  {
		ValueStepSequence test = ValueStepSequence.of(date(2016, 4, 20), date(2016, 10, 20), Frequency.P12M, ADJ);
		ValueStep baseStep = ValueStep.of(date(2016, 1, 20), ValueAdjustment.ofReplace(500d));
		assertThrowsIllegalArg(() => test.resolve(ImmutableList.of(baseStep), RollConventions.NONE));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ValueStepSequence test = ValueStepSequence.of(date(2016, 4, 20), date(2016, 10, 20), Frequency.P3M, ADJ);
		coverImmutableBean(test);
		ValueStepSequence test2 = ValueStepSequence.of(date(2016, 4, 1), date(2016, 10, 1), Frequency.P1M, ADJ2);
		coverImmutableBean(test2);
	  }

	  public virtual void test_serialization()
	  {
		ValueStepSequence test = ValueStepSequence.of(date(2016, 4, 20), date(2016, 10, 20), Frequency.P3M, ADJ);
		assertSerialization(test);
	  }

	}

}