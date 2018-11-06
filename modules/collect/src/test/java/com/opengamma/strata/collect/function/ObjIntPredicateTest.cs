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
	/// Test ObjIntPredicate.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ObjIntPredicateTest
	public class ObjIntPredicateTest
	{

	  public virtual void test_and()
	  {
		ObjIntPredicate<string> fn1 = (a, b) => b > 3;
		ObjIntPredicate<string> fn2 = (a, b) => a.length() > 3;
		ObjIntPredicate<string> and = fn1.and(fn2);
		assertEquals(fn1.test("a", 2), false);
		assertEquals(fn1.test("a", 4), true);
		assertEquals(fn2.test("a", 4), false);
		assertEquals(fn2.test("abcd", 4), true);
		assertEquals(and.test("a", 2), false);
		assertEquals(and.test("a", 4), false);
		assertEquals(and.test("abcd", 2), false);
		assertEquals(and.test("abcd", 4), true);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = NullPointerException.class) public void test_and_null()
	  public virtual void test_and_null()
	  {
		ObjIntPredicate<string> fn1 = (a, b) => b > 3;
		fn1.and(null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_or()
	  {
		ObjIntPredicate<string> fn1 = (a, b) => b > 3;
		ObjIntPredicate<string> fn2 = (a, b) => a.length() > 3;
		ObjIntPredicate<string> or = fn1.or(fn2);
		assertEquals(fn1.test("a", 2), false);
		assertEquals(fn1.test("a", 4), true);
		assertEquals(fn2.test("a", 4), false);
		assertEquals(fn2.test("abcd", 4), true);
		assertEquals(or.test("a", 2), false);
		assertEquals(or.test("a", 4), true);
		assertEquals(or.test("abcd", 2), true);
		assertEquals(or.test("abcd", 4), true);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = NullPointerException.class) public void test_or_null()
	  public virtual void test_or_null()
	  {
		ObjIntPredicate<string> fn1 = (a, b) => b > 3;
		fn1.or(null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_negate()
	  {
		ObjIntPredicate<string> fn1 = (a, b) => b > 3;
		ObjIntPredicate<string> negate = fn1.negate();
		assertEquals(fn1.test("a", 2), false);
		assertEquals(fn1.test("a", 4), true);
		assertEquals(negate.test("a", 2), true);
		assertEquals(negate.test("a", 4), false);
	  }

	}

}