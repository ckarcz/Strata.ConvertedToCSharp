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
	/// Test ObjDoubleFunction.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ObjDoubleFunctionTest
	public class ObjDoubleFunctionTest
	{

	  public virtual void test_andThen()
	  {
		ObjDoubleFunction<int, string> fn1 = (a, b) => a + "=" + b;
		ObjDoubleFunction<int, string> fn2 = fn1.andThen(str => "[" + str + "]");
		assertEquals(fn1.apply(2, 3.2d), "2=3.2");
		assertEquals(fn2.apply(2, 3.2d), "[2=3.2]");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = NullPointerException.class) public void test_andThen_null()
	  public virtual void test_andThen_null()
	  {
		ObjDoubleFunction<int, string> fn1 = (a, b) => a + "=" + b;
		fn1.andThen(null);
	  }

	}

}