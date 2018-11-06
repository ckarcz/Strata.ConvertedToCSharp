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
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsWithCause;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverEnum;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="CurveNodeDate"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveNodeDateTest
	public class CurveNodeDateTest
	{

	  private static readonly LocalDate DATE1 = date(2015, 6, 30);
	  private static readonly LocalDate DATE2 = date(2015, 7, 1);
	  private static readonly LocalDate DATE3 = date(2015, 7, 2);

	  //-------------------------------------------------------------------------
	  public virtual void test_END()
	  {
		CurveNodeDate test = CurveNodeDate.END;
		assertEquals(test.Fixed, false);
		assertEquals(test.End, true);
		assertEquals(test.LastFixing, false);
		assertEquals(test.Type, CurveNodeDateType.END);
		assertThrowsWithCause(() => test.Date, typeof(System.InvalidOperationException));
	  }

	  public virtual void test_LAST_FIXING()
	  {
		CurveNodeDate test = CurveNodeDate.LAST_FIXING;
		assertEquals(test.Fixed, false);
		assertEquals(test.End, false);
		assertEquals(test.LastFixing, true);
		assertEquals(test.Type, CurveNodeDateType.LAST_FIXING);
		assertThrowsWithCause(() => test.Date, typeof(System.InvalidOperationException));
	  }

	  public virtual void test_of()
	  {
		CurveNodeDate test = CurveNodeDate.of(DATE1);
		assertEquals(test.Fixed, true);
		assertEquals(test.End, false);
		assertEquals(test.LastFixing, false);
		assertEquals(test.Type, CurveNodeDateType.FIXED);
		assertEquals(test.Date, DATE1);
	  }

	  public virtual void test_builder_fixed()
	  {
		CurveNodeDate test = CurveNodeDate.meta().builder().set(CurveNodeDate.meta().type(), CurveNodeDateType.FIXED).set(CurveNodeDate.meta().date(), DATE1).build();
		assertEquals(test.Fixed, true);
		assertEquals(test.End, false);
		assertEquals(test.LastFixing, false);
		assertEquals(test.Type, CurveNodeDateType.FIXED);
		assertEquals(test.Date, DATE1);
	  }

	  public virtual void test_builder_incorrect_no_fixed_date()
	  {
		assertThrowsIllegalArg(() => CurveNodeDate.meta().builder().set(CurveNodeDate.meta().type(), CurveNodeDateType.FIXED).build());
	  }

	  public virtual void test_builder_incorrect_fixed_date()
	  {
		assertThrowsIllegalArg(() => CurveNodeDate.meta().builder().set(CurveNodeDate.meta().type(), CurveNodeDateType.LAST_FIXING).set(CurveNodeDate.meta().date(), DATE1).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_calculate()
	  {
		assertEquals(CurveNodeDate.of(DATE1).calculate(() => DATE2, () => DATE3), DATE1);
		assertEquals(CurveNodeDate.END.calculate(() => DATE2, () => DATE3), DATE2);
		assertEquals(CurveNodeDate.LAST_FIXING.calculate(() => DATE2, () => DATE3), DATE3);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CurveNodeDate test = CurveNodeDate.of(DATE1);
		coverImmutableBean(test);
		CurveNodeDate test2 = CurveNodeDate.LAST_FIXING;
		coverBeanEquals(test, test2);
		coverEnum(typeof(CurveNodeDateType));
	  }

	  public virtual void test_serialization()
	  {
		CurveNodeDate test = CurveNodeDate.of(DATE1);
		assertSerialization(test);
	  }

	}

}