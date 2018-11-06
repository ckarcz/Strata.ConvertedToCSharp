/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.function
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test ObjLongPredicate.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ObjLongPredicateTest
	public class ObjLongPredicateTest
	{

	  public virtual void test_and()
	  {
		ObjLongPredicate<string> fn1 = (a, b) => b > 3;
		ObjLongPredicate<string> fn2 = (a, b) => a.length() > 3;
		ObjLongPredicate<string> and = fn1.and(fn2);
		assertEquals(fn1.test("a", 2L), false);
		assertEquals(fn1.test("a", 4L), true);
		assertEquals(fn2.test("a", 4L), false);
		assertEquals(fn2.test("abcd", 4L), true);
		assertEquals(and.test("a", 2L), false);
		assertEquals(and.test("a", 4L), false);
		assertEquals(and.test("abcd", 2L), false);
		assertEquals(and.test("abcd", 4L), true);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = NullPointerException.class) public void test_and_null()
	  public virtual void test_and_null()
	  {
		ObjLongPredicate<string> fn1 = (a, b) => b > 3;
		fn1.and(null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_or()
	  {
		ObjLongPredicate<string> fn1 = (a, b) => b > 3;
		ObjLongPredicate<string> fn2 = (a, b) => a.length() > 3;
		ObjLongPredicate<string> or = fn1.or(fn2);
		assertEquals(fn1.test("a", 2L), false);
		assertEquals(fn1.test("a", 4L), true);
		assertEquals(fn2.test("a", 4L), false);
		assertEquals(fn2.test("abcd", 4L), true);
		assertEquals(or.test("a", 2L), false);
		assertEquals(or.test("a", 4L), true);
		assertEquals(or.test("abcd", 2L), true);
		assertEquals(or.test("abcd", 4L), true);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = NullPointerException.class) public void test_or_null()
	  public virtual void test_or_null()
	  {
		ObjLongPredicate<string> fn1 = (a, b) => b > 3;
		fn1.or(null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_negate()
	  {
		ObjLongPredicate<string> fn1 = (a, b) => b > 3;
		ObjLongPredicate<string> negate = fn1.negate();
		assertEquals(fn1.test("a", 2L), false);
		assertEquals(fn1.test("a", 4L), true);
		assertEquals(negate.test("a", 2L), true);
		assertEquals(negate.test("a", 4L), false);
	  }

	}

}