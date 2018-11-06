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
	/// Test ObjDoublePredicate.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ObjDoublePredicateTest
	public class ObjDoublePredicateTest
	{

	  public virtual void test_and()
	  {
		ObjDoublePredicate<string> fn1 = (a, b) => b > 3;
		ObjDoublePredicate<string> fn2 = (a, b) => a.length() > 3;
		ObjDoublePredicate<string> and = fn1.and(fn2);
		assertEquals(fn1.test("a", 2.3d), false);
		assertEquals(fn1.test("a", 3.2d), true);
		assertEquals(fn2.test("a", 3.2d), false);
		assertEquals(fn2.test("abcd", 3.2d), true);
		assertEquals(and.test("a", 2.3d), false);
		assertEquals(and.test("a", 3.2d), false);
		assertEquals(and.test("abcd", 2.3d), false);
		assertEquals(and.test("abcd", 3.2d), true);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = NullPointerException.class) public void test_and_null()
	  public virtual void test_and_null()
	  {
		ObjDoublePredicate<string> fn1 = (a, b) => b > 3;
		fn1.and(null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_or()
	  {
		ObjDoublePredicate<string> fn1 = (a, b) => b > 3;
		ObjDoublePredicate<string> fn2 = (a, b) => a.length() > 3;
		ObjDoublePredicate<string> or = fn1.or(fn2);
		assertEquals(fn1.test("a", 2.3d), false);
		assertEquals(fn1.test("a", 3.2d), true);
		assertEquals(fn2.test("a", 3.2d), false);
		assertEquals(fn2.test("abcd", 3.2d), true);
		assertEquals(or.test("a", 2.3d), false);
		assertEquals(or.test("a", 3.2d), true);
		assertEquals(or.test("abcd", 2.3d), true);
		assertEquals(or.test("abcd", 3.2d), true);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = NullPointerException.class) public void test_or_null()
	  public virtual void test_or_null()
	  {
		ObjDoublePredicate<string> fn1 = (a, b) => b > 3;
		fn1.or(null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_negate()
	  {
		ObjDoublePredicate<string> fn1 = (a, b) => b > 3;
		ObjDoublePredicate<string> negate = fn1.negate();
		assertEquals(fn1.test("a", 2.3d), false);
		assertEquals(fn1.test("a", 3.2d), true);
		assertEquals(negate.test("a", 2.3d), true);
		assertEquals(negate.test("a", 3.2d), false);
	  }

	}

}