using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.result
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Test <seealso cref="ValueWithFailures"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ValueWithFailuresTest
	public class ValueWithFailuresTest
	{

	  private static readonly FailureItem FAILURE1 = FailureItem.of(FailureReason.INVALID, "invalid");
	  private static readonly FailureItem FAILURE2 = FailureItem.of(FailureReason.MISSING_DATA, "data");

	  //-------------------------------------------------------------------------
	  public virtual void test_of_array_noFailures()
	  {
		ValueWithFailures<string> test = ValueWithFailures.of("success");
		assertEquals(test.hasFailures(), false);
		assertEquals(test.Value, "success");
		assertEquals(test.Failures, ImmutableList.of());
	  }

	  public virtual void test_of_array()
	  {
		ValueWithFailures<string> test = ValueWithFailures.of("success", FAILURE1, FAILURE2);
		assertEquals(test.hasFailures(), true);
		assertEquals(test.Value, "success");
		assertEquals(test.Failures, ImmutableList.of(FAILURE1, FAILURE2));
	  }

	  public virtual void test_of_list()
	  {
		ValueWithFailures<string> test = ValueWithFailures.of("success", ImmutableList.of(FAILURE1, FAILURE2));
		assertEquals(test.hasFailures(), true);
		assertEquals(test.Value, "success");
		assertEquals(test.Failures, ImmutableList.of(FAILURE1, FAILURE2));
	  }

	  public virtual void test_of_supplier_success()
	  {
		ValueWithFailures<string> test = ValueWithFailures.of("", () => "A");
		assertEquals(test.hasFailures(), false);
		assertEquals(test.Value, "A");
		assertEquals(test.Failures, ImmutableList.of());
	  }

	  public virtual void test_of_supplier_failure()
	  {
		ValueWithFailures<string> test = ValueWithFailures.of("", () =>
		{
		throw new System.ArgumentException();
		});
		assertEquals(test.hasFailures(), true);
		assertEquals(test.Value, "");
		assertEquals(test.Failures.size(), 1);
		assertEquals(test.Failures.get(0).Reason, FailureReason.ERROR);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_map()
	  {
		ValueWithFailures<IList<string>> @base = ValueWithFailures.of(ImmutableList.of("1", "2"), ImmutableList.of(FAILURE1));
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ValueWithFailures<IList<int>> test = @base.map(list => list.Select(s => Convert.ToInt32(s)).collect(toImmutableList()));
		assertEquals(test.Value, ImmutableList.of(Convert.ToInt32(1), Convert.ToInt32(2)));
		assertEquals(test.Failures, ImmutableList.of(FAILURE1));
	  }

	  public virtual void test_flatMap()
	  {
		ValueWithFailures<IList<string>> @base = ValueWithFailures.of(ImmutableList.of("1", "a", "2"), ImmutableList.of(FAILURE1));
		ValueWithFailures<IList<int>> test = @base.flatMap(this.flatMapFunction);
		assertEquals(test.Value, ImmutableList.of(Convert.ToInt32(1), Convert.ToInt32(2)));
		assertEquals(test.Failures.size(), 2);
		assertEquals(test.Failures.get(0), FAILURE1);
		assertEquals(test.Failures.get(1).Reason, FailureReason.INVALID);
	  }

	  private ValueWithFailures<IList<int>> flatMapFunction(IList<string> input)
	  {
		IList<int> integers = new List<int>();
		IList<FailureItem> failures = new List<FailureItem>();
		foreach (string str in input)
		{
		  try
		  {
			integers.Add(Convert.ToInt32(str));
		  }
		  catch (System.FormatException ex)
		  {
			failures.Add(FailureItem.of(FailureReason.INVALID, ex));
		  }
		}
		return ValueWithFailures.of(integers, failures);
	  }

	  // -------------------------------------------------------------------------
	  public virtual void test_combinedWith()
	  {
		ValueWithFailures<IList<string>> @base = ValueWithFailures.of(ImmutableList.of("a"), ImmutableList.of(FAILURE1));
		ValueWithFailures<IList<string>> other = ValueWithFailures.of(ImmutableList.of("b", "c"), ImmutableList.of(FAILURE2));
		ValueWithFailures<IList<string>> test = @base.combinedWith(other, Guavate.concatToList);
		assertEquals(test.Value, ImmutableList.of("a", "b", "c"));
		assertEquals(test.Failures, ImmutableList.of(FAILURE1, FAILURE2));
	  }

	  public virtual void test_combinedWith_differentTypes()
	  {
		ValueWithFailures<bool> @base = ValueWithFailures.of(true, ImmutableList.of(FAILURE1));
		ValueWithFailures<int> other = ValueWithFailures.of(Convert.ToInt32(1), ImmutableList.of(FAILURE2));
		ValueWithFailures<string> test = @base.combinedWith(other, (a, b) => a.ToString() + b.ToString());
		assertEquals(test.Value, "true1");
		assertEquals(test.Failures, ImmutableList.of(FAILURE1, FAILURE2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toValueWithFailures()
	  {
		IList<double> testList = ImmutableList.of(5d, 6d, 7d);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ValueWithFailures<double> result = testList.Select(value => mockCalc(value)).collect(ValueWithFailures.toValueWithFailures(1d, (val1, val2) => val1 * val2));

		assertEquals(result.Value, 210d); //5 * 6 * 7 = 210
		IList<FailureItem> failures = result.Failures;
		assertEquals(failures.Count, 3); //One failure item for each element in testList.
		assertEquals(failures[0].Message, Messages.format("Error calculating result for input value {}", 5d));
		assertEquals(failures[1].Message, Messages.format("Error calculating result for input value {}", 6d));
		assertEquals(failures[2].Message, Messages.format("Error calculating result for input value {}", 7d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ValueWithFailures<string> test = ValueWithFailures.of("success", FAILURE1, FAILURE2);
		coverImmutableBean(test);
		ValueWithFailures<string> test2 = ValueWithFailures.of("test");
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ValueWithFailures<string> test = ValueWithFailures.of("success", FAILURE1, FAILURE2);
		assertSerialization(test);
	  }

	  private static ValueWithFailures<double> mockCalc(double value)
	  {
		FailureItem failure = FailureItem.of(FailureReason.CALCULATION_FAILED, "Error calculating result for input value {}", value);

		return ValueWithFailures.of(value, ImmutableList.of(failure));
	  }

	}

}