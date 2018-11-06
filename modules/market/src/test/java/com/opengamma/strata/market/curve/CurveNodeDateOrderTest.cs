/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
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
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="CurveNodeDateOrder"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveNodeDateOrderTest
	public class CurveNodeDateOrderTest
	{

	  public virtual void test_DEFAULT()
	  {
		CurveNodeDateOrder test = CurveNodeDateOrder.DEFAULT;
		assertEquals(test.MinGapInDays, 1);
		assertEquals(test.Action, CurveNodeClashAction.EXCEPTION);
	  }

	  public virtual void test_of()
	  {
		CurveNodeDateOrder test = CurveNodeDateOrder.of(2, CurveNodeClashAction.DROP_THIS);
		assertEquals(test.MinGapInDays, 2);
		assertEquals(test.Action, CurveNodeClashAction.DROP_THIS);
	  }

	  public virtual void test_of_invalid()
	  {
		assertThrowsIllegalArg(() => CurveNodeDateOrder.of(0, CurveNodeClashAction.DROP_THIS));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CurveNodeDateOrder test = CurveNodeDateOrder.of(2, CurveNodeClashAction.DROP_THIS);
		coverImmutableBean(test);
		CurveNodeDateOrder test2 = CurveNodeDateOrder.of(3, CurveNodeClashAction.DROP_OTHER);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		CurveNodeDateOrder test = CurveNodeDateOrder.of(2, CurveNodeClashAction.DROP_THIS);
		assertSerialization(test);
	  }

	}

}