using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.ComplexMathUtils.multiply;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ComplexMathUtilsTest
	public class ComplexMathUtilsTest
	{
	  private const double V = 0.123;
	  private const double W = 0.456;
	  private const double X = 7.89;
	  private const double Y = -12.34;
	  private static readonly ComplexNumber X_C = new ComplexNumber(X, 0);
	  private static readonly ComplexNumber Z1 = new ComplexNumber(V, W);
	  private static readonly ComplexNumber Z2 = new ComplexNumber(X, Y);
	  private const double EPS = 1e-9;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNull()
	  public virtual void testNull()
	  {
		try
		{
		  ComplexMathUtils.add(null, Z1);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.add(Z1, null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.add(X, null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.add(null, X);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.arg(null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.conjugate(null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.divide(null, Z1);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.divide(Z1, null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.divide(X, null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.divide(null, X);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.exp(null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.inverse(null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.log(null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.mod(null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.multiply(null, Z1);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.multiply(Z1, null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.multiply(X, (ComplexNumber) null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.multiply(null, X);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.pow(null, Z1);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.pow(Z1, null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.pow(X, null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.pow(null, X);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.sqrt(null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.subtract(null, Z1);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.subtract(Z1, null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.subtract(X, null);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
		try
		{
		  ComplexMathUtils.subtract(null, X);
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (System.ArgumentException e)
		{
		  assertStackTraceElement(e.StackTrace);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testAddSubtract()
	  public virtual void testAddSubtract()
	  {
		assertComplexEquals(ComplexMathUtils.subtract(ComplexMathUtils.add(Z1, Z2), Z2), Z1);
		assertComplexEquals(ComplexMathUtils.subtract(ComplexMathUtils.add(Z1, X), X), Z1);
		assertComplexEquals(ComplexMathUtils.subtract(ComplexMathUtils.add(X, Z1), Z1), X_C);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testArg()
	  public virtual void testArg()
	  {
		assertEquals(Math.Atan2(W, V), ComplexMathUtils.arg(Z1), EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testConjugate()
	  public virtual void testConjugate()
	  {
		assertComplexEquals(ComplexMathUtils.conjugate(ComplexMathUtils.conjugate(Z1)), Z1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testDivideMultiply()
	  public virtual void testDivideMultiply()
	  {
		assertComplexEquals(ComplexMathUtils.multiply(ComplexMathUtils.divide(Z1, Z2), Z2), Z1);
		assertComplexEquals(ComplexMathUtils.multiply(ComplexMathUtils.divide(Z1, X), X), Z1);
		assertComplexEquals(ComplexMathUtils.multiply(ComplexMathUtils.divide(X, Z1), Z1), X_C);
		assertComplexEquals(ComplexMathUtils.multiply(X, Z1), ComplexMathUtils.multiply(Z1, X));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testMultiplyMany()
	  public virtual void testMultiplyMany()
	  {
		ComplexNumber a = multiply(Z1, multiply(Z2, Z1));
		ComplexNumber b = multiply(Z1, Z2, Z1);
		assertComplexEquals(a, b);
		double x = 3.142;
		ComplexNumber c = multiply(a, x);
		ComplexNumber d = multiply(x, Z1, Z1, Z2);
		assertComplexEquals(c, d);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testExpLn()
	  public virtual void testExpLn()
	  {
		assertComplexEquals(ComplexMathUtils.log(ComplexMathUtils.exp(Z1)), Z1);
		//TODO test principal value
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testInverse()
	  public virtual void testInverse()
	  {
		assertComplexEquals(ComplexMathUtils.inverse(ComplexMathUtils.inverse(Z1)), Z1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testModulus()
	  public virtual void testModulus()
	  {
		assertEquals(Math.Sqrt(V * V + W * W), ComplexMathUtils.mod(Z1), EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testPower()
	  public virtual void testPower()
	  {
		assertComplexEquals(ComplexMathUtils.pow(Z1, 0), new ComplexNumber(1, 0));
		assertComplexEquals(ComplexMathUtils.pow(X, new ComplexNumber(0, 0)), new ComplexNumber(1, 0));
		assertComplexEquals(ComplexMathUtils.sqrt(ComplexMathUtils.pow(Z1, 2)), Z1);
		assertComplexEquals(ComplexMathUtils.sqrt(ComplexMathUtils.pow(Z2, 2)), Z2);
		assertComplexEquals(ComplexMathUtils.pow(ComplexMathUtils.pow(Z1, 1.0 / 3), 3), Z1);
		assertComplexEquals(ComplexMathUtils.pow(ComplexMathUtils.pow(X, ComplexMathUtils.inverse(Z2)), Z2), new ComplexNumber(X, 0));
		assertComplexEquals(ComplexMathUtils.pow(ComplexMathUtils.pow(Z1, ComplexMathUtils.inverse(Z2)), Z2), Z1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testSqrt()
	  public virtual void testSqrt()
	  {
		ComplexNumber z1 = new ComplexNumber(3, -2);
		ComplexNumber z2 = new ComplexNumber(-3, 4);
		ComplexNumber z3 = new ComplexNumber(-3, -4);

		ComplexNumber rZ1 = ComplexMathUtils.sqrt(z1);
		ComplexNumber rZ2 = ComplexMathUtils.sqrt(z2);
		ComplexNumber rZ3 = ComplexMathUtils.sqrt(z3);

		assertComplexEquals(ComplexMathUtils.pow(z1, 0.5), rZ1);
		assertComplexEquals(ComplexMathUtils.pow(z2, 0.5), rZ2);
		assertComplexEquals(ComplexMathUtils.pow(z3, 0.5), rZ3);

		assertComplexEquals(z1, ComplexMathUtils.square(rZ1));
		assertComplexEquals(z2, ComplexMathUtils.square(rZ2));
		assertComplexEquals(z3, ComplexMathUtils.square(rZ3));
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private void assertComplexEquals(final ComplexNumber z1, final ComplexNumber z2)
	  private void assertComplexEquals(ComplexNumber z1, ComplexNumber z2)
	  {
		assertEquals(z1.Real, z2.Real, EPS);
		assertEquals(z1.Imaginary, z2.Imaginary, EPS);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private void assertStackTraceElement(final StackTraceElement[] ste)
	  private void assertStackTraceElement(StackTraceElement[] ste)
	  {
		assertEquals(ste[0].ClassName, "com.opengamma.strata.collect.ArgChecker");
	  }
	}

}