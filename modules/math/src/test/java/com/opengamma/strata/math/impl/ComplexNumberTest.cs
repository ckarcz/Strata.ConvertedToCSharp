using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertFalse;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ComplexNumberTest
	public class ComplexNumberTest
	{

	  private static readonly ComplexNumber Z1 = new ComplexNumber(1, 2);
	  private static readonly ComplexNumber Z2 = new ComplexNumber(1, 2);
	  private static readonly ComplexNumber Z3 = new ComplexNumber(1, 3);
	  private static readonly ComplexNumber Z4 = new ComplexNumber(2, 2);
	  private static readonly ComplexNumber Z5 = new ComplexNumber(2, 3);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testByteValue()
	  public virtual void testByteValue()
	  {
		Z1.byteValue();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testIntValue()
	  public virtual void testIntValue()
	  {
		Z1.intValue();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testLongValue()
	  public virtual void testLongValue()
	  {
		Z1.longValue();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testFloatValue()
	  public virtual void testFloatValue()
	  {
		Z1.floatValue();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = UnsupportedOperationException.class) public void testDoubleValue()
	  public virtual void testDoubleValue()
	  {
		Z1.doubleValue();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertEquals(Convert.ToDouble(1), Convert.ToDouble(Z1.Real));
		assertEquals(Convert.ToDouble(2), Convert.ToDouble(Z1.Imaginary));
		assertEquals(Z1, Z2);
		assertEquals(Z1.GetHashCode(), Z2.GetHashCode());
		assertEquals("1.0 + 2.0i", Z1.ToString());
		assertEquals("1.0 + 0.0i", (new ComplexNumber(1, 0)).ToString());
		assertEquals("0.0 + 2.3i", (new ComplexNumber(0, 2.3)).ToString());
		assertEquals("-1.0 + 0.0i", (new ComplexNumber(-1, 0)).ToString());
		assertEquals("0.0 - 2.3i", (new ComplexNumber(0, -2.3)).ToString());
		assertFalse(Z1.Equals(Z3));
		assertFalse(Z1.Equals(Z4));
		assertFalse(Z1.Equals(Z5));
	  }
	}

}