/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.result
{
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
	/// Test <seealso cref="FailureItems"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FailureItemsTest
	public class FailureItemsTest
	{

	  private static readonly FailureItem FAILURE1 = FailureItem.of(FailureReason.INVALID, "invalid");
	  private static readonly FailureItem FAILURE2 = FailureItem.of(FailureReason.MISSING_DATA, "data");

	  //-------------------------------------------------------------------------
	  public virtual void test_EMPTY()
	  {
		FailureItems test = FailureItems.EMPTY;
		assertEquals(test.Empty, true);
		assertEquals(test.Failures, ImmutableList.of());
	  }

	  public virtual void test_of_array()
	  {
		FailureItems test = FailureItems.of(FAILURE1, FAILURE2);
		assertEquals(test.Empty, false);
		assertEquals(test.Failures, ImmutableList.of(FAILURE1, FAILURE2));
	  }

	  public virtual void test_of_list()
	  {
		FailureItems test = FailureItems.of(ImmutableList.of(FAILURE1, FAILURE2));
		assertEquals(test.Empty, false);
		assertEquals(test.Failures, ImmutableList.of(FAILURE1, FAILURE2));
	  }

	  public virtual void test_builder_add()
	  {
		FailureItems test = FailureItems.builder().addFailure(FAILURE1).addFailure(FAILURE2).build();
		assertEquals(test.Empty, false);
		assertEquals(test.Failures, ImmutableList.of(FAILURE1, FAILURE2));
	  }

	  public virtual void test_builder_addAll()
	  {
		FailureItems test = FailureItems.builder().addAllFailures(ImmutableList.of(FAILURE1, FAILURE2)).build();
		assertEquals(test.Empty, false);
		assertEquals(test.Failures, ImmutableList.of(FAILURE1, FAILURE2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FailureItems test = FailureItems.of(FAILURE1, FAILURE2);
		coverImmutableBean(test);
		FailureItems test2 = FailureItems.EMPTY;
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		FailureItems test = FailureItems.of(FAILURE1, FAILURE2);
		assertSerialization(test);
	  }

	}

}