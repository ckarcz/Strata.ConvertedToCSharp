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

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TrigonometricFunctionUtilsTest
	public class TrigonometricFunctionUtilsTest
	{
	  private const double? X = 0.12;
	  private static readonly ComplexNumber Y = new ComplexNumber(X.Value, 0);
	  private static readonly ComplexNumber Z = new ComplexNumber(X.Value, -0.34);
	  private const double EPS = 1e-9;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull1()
	  public virtual void testNull1()
	  {
		TrigonometricFunctionUtils.acos(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull2()
	  public virtual void testNull2()
	  {
		TrigonometricFunctionUtils.acosh(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull3()
	  public virtual void testNull3()
	  {
		TrigonometricFunctionUtils.asin(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull4()
	  public virtual void testNull4()
	  {
		TrigonometricFunctionUtils.asinh(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull5()
	  public virtual void testNull5()
	  {
		TrigonometricFunctionUtils.atan(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull6()
	  public virtual void testNull6()
	  {
		TrigonometricFunctionUtils.atanh(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull7()
	  public virtual void testNull7()
	  {
		TrigonometricFunctionUtils.cos(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull8()
	  public virtual void testNull8()
	  {
		TrigonometricFunctionUtils.cosh(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull9()
	  public virtual void testNull9()
	  {
		TrigonometricFunctionUtils.sin(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull10()
	  public virtual void testNull10()
	  {
		TrigonometricFunctionUtils.sinh(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull11()
	  public virtual void testNull11()
	  {
		TrigonometricFunctionUtils.tan(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull12()
	  public virtual void testNull12()
	  {
		TrigonometricFunctionUtils.tanh(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertEquals(TrigonometricFunctionUtils.acos(TrigonometricFunctionUtils.cos(X)), X, EPS);
		assertEquals(TrigonometricFunctionUtils.asin(TrigonometricFunctionUtils.sin(X)), X, EPS);
		assertEquals(TrigonometricFunctionUtils.atan(TrigonometricFunctionUtils.tan(X)), X, EPS);
		assertComplexEquals(TrigonometricFunctionUtils.cos(Y), Math.Cos(X));
		assertComplexEquals(TrigonometricFunctionUtils.sin(Y), Math.Sin(X));
		assertComplexEquals(TrigonometricFunctionUtils.tan(Y), Math.Tan(X));
		assertComplexEquals(TrigonometricFunctionUtils.acos(Y), Math.Acos(X));
		assertComplexEquals(TrigonometricFunctionUtils.asin(Y), Math.Asin(X));
		assertComplexEquals(TrigonometricFunctionUtils.atan(Y), Math.Atan(X));
		assertComplexEquals(TrigonometricFunctionUtils.acos(TrigonometricFunctionUtils.cos(Z)), Z);
		assertComplexEquals(TrigonometricFunctionUtils.asin(TrigonometricFunctionUtils.sin(Z)), Z);
		assertComplexEquals(TrigonometricFunctionUtils.atan(TrigonometricFunctionUtils.tan(Z)), Z);
		assertEquals(TrigonometricFunctionUtils.acosh(TrigonometricFunctionUtils.cosh(X)), X, EPS);
		assertEquals(TrigonometricFunctionUtils.asinh(TrigonometricFunctionUtils.sinh(X)), X, EPS);
		assertEquals(TrigonometricFunctionUtils.atanh(TrigonometricFunctionUtils.tanh(X)), X, EPS);
		assertComplexEquals(TrigonometricFunctionUtils.acosh(TrigonometricFunctionUtils.cosh(Z)), Z);
		assertComplexEquals(TrigonometricFunctionUtils.asinh(TrigonometricFunctionUtils.sinh(Z)), Z);
		assertComplexEquals(TrigonometricFunctionUtils.atanh(TrigonometricFunctionUtils.tanh(Z)), Z);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testAtanh()
	  public virtual void testAtanh()
	  {
		double x = 0.76;
		ComplexNumber z = new ComplexNumber(x);
		double real = 0.5 * Math.Log((1 + x) / (1 - x));
		ComplexNumber res = TrigonometricFunctionUtils.atanh(z);
		assertEquals(real, res.Real, 1e-15);
		assertEquals(0.0, res.Imaginary, 0);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private void assertComplexEquals(final ComplexNumber z1, final ComplexNumber z2)
	  private void assertComplexEquals(ComplexNumber z1, ComplexNumber z2)
	  {
		assertEquals(z1.Real, z2.Real, EPS);
		assertEquals(z1.Imaginary, z2.Imaginary, EPS);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private void assertComplexEquals(final ComplexNumber z, final double x)
	  private void assertComplexEquals(ComplexNumber z, double x)
	  {
		assertEquals(z.Imaginary, 0, EPS);
		assertEquals(z.Real, x, EPS);
	  }
	}

}