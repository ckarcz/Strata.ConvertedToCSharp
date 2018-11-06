using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using FiniteDifferenceType = com.opengamma.strata.math.impl.differentiation.FiniteDifferenceType;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DoubleFunction1DTest
	public class DoubleFunction1DTest
	{

	  private static readonly DoubleFunction1D F1 = x => x * x * x + 2 * x * x - 7 * x + 12;
	  private static readonly DoubleFunction1D DF1 = x => 3 * x * x + 4 * x - 7;
	  private static readonly DoubleFunction1D F2 = x => Math.Sin(x);
	  private static readonly DoubleFunction1D DF2 = x => Math.Cos(x);
	  private static readonly DoubleFunction1D F3 = new DoubleFunction1DAnonymousInnerClass();

	  private class DoubleFunction1DAnonymousInnerClass : DoubleFunction1D
	  {
		  public DoubleFunction1DAnonymousInnerClass()
		  {
		  }


		  public override double applyAsDouble(double x)
		  {
			return x * x * x + 2 * x * x - 7 * x + 12;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public DoubleFunction1D derivative()
		  public override DoubleFunction1D derivative()
		  {
			return DF1;
		  }
	  }
	  private static readonly DoubleFunction1D F4 = new DoubleFunction1DAnonymousInnerClass2();

	  private class DoubleFunction1DAnonymousInnerClass2 : DoubleFunction1D
	  {
		  public DoubleFunction1DAnonymousInnerClass2()
		  {
		  }


		  public override double applyAsDouble(double x)
		  {
			return Math.Sin(x);
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public DoubleFunction1D derivative()
		  public override DoubleFunction1D derivative()
		  {
			return DF2;
		  }
	  }
	  private const double X = 0.1234;
	  private const double A = 5.67;
	  private const double EPS = 1e-15;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testAddNull()
	  public virtual void testAddNull()
	  {
		F1.add(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDivideNull()
	  public virtual void testDivideNull()
	  {
		F1.divide(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testMultiplyNull()
	  public virtual void testMultiplyNull()
	  {
		F1.multiply(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testSubtractNull()
	  public virtual void testSubtractNull()
	  {
		F1.subtract(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testConvertNull()
	  public virtual void testConvertNull()
	  {
		DoubleFunction1D.from(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDerivativeNullType()
	  public virtual void testDerivativeNullType()
	  {
		F1.derivative(null, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testAdd()
	  public virtual void testAdd()
	  {
		assertEquals(F1.add(F2).applyAsDouble(X), F1.applyAsDouble(X) + F2.applyAsDouble(X), EPS);
		assertEquals(F1.add(A).applyAsDouble(X), F1.applyAsDouble(X) + A, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testDivide()
	  public virtual void testDivide()
	  {
		assertEquals(F1.divide(F2).applyAsDouble(X), F1.applyAsDouble(X) / F2.applyAsDouble(X), EPS);
		assertEquals(F1.divide(A).applyAsDouble(X), F1.applyAsDouble(X) / A, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testMultiply()
	  public virtual void testMultiply()
	  {
		assertEquals(F1.multiply(F2).applyAsDouble(X), F1.applyAsDouble(X) * F2.applyAsDouble(X), EPS);
		assertEquals(F1.multiply(A).applyAsDouble(X), F1.applyAsDouble(X) * A, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testSubtract()
	  public virtual void testSubtract()
	  {
		assertEquals(F1.subtract(F2).applyAsDouble(X), F1.applyAsDouble(X) - F2.applyAsDouble(X), EPS);
		assertEquals(F1.subtract(A).applyAsDouble(X), F1.applyAsDouble(X) - A, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testDerivative()
	  public virtual void testDerivative()
	  {
		assertEquals(F1.derivative().applyAsDouble(X), DF1.applyAsDouble(X), 1e-3);
		assertEquals(F2.derivative().applyAsDouble(X), DF2.applyAsDouble(X), 1e-3);
		assertEquals(F1.derivative(FiniteDifferenceType.CENTRAL, 1e-5).applyAsDouble(X), DF1.applyAsDouble(X), 1e-3);
		assertEquals(F2.derivative(FiniteDifferenceType.CENTRAL, 1e-5).applyAsDouble(X), DF2.applyAsDouble(X), 1e-3);
		assertEquals(F1.derivative(FiniteDifferenceType.FORWARD, 1e-5).applyAsDouble(X), DF1.applyAsDouble(X), 1e-3);
		assertEquals(F2.derivative(FiniteDifferenceType.FORWARD, 1e-5).applyAsDouble(X), DF2.applyAsDouble(X), 1e-3);
		assertEquals(F1.derivative(FiniteDifferenceType.BACKWARD, 1e-5).applyAsDouble(X), DF1.applyAsDouble(X), 1e-3);
		assertEquals(F2.derivative(FiniteDifferenceType.BACKWARD, 1e-5).applyAsDouble(X), DF2.applyAsDouble(X), 1e-3);
		assertEquals(F3.derivative().applyAsDouble(X), DF1.applyAsDouble(X), 1e-15);
		assertEquals(F4.derivative().applyAsDouble(X), DF2.applyAsDouble(X), 1e-15);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testConversion()
	  public virtual void testConversion()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> f1 = x -> x * x * x + 2 * x * x - 7 * x + 12;
		System.Func<double, double> f1 = x => x * x * x + 2 * x * x - 7 * x + 12;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final DoubleFunction1D f2 = DoubleFunction1D.from(f1);
		DoubleFunction1D f2 = DoubleFunction1D.from(f1);
		for (int i = 0; i < 100; i++)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = Math.random();
		  double x = GlobalRandom.NextDouble;
		  assertEquals(f2.applyAsDouble(x), F1.applyAsDouble(x), 0);
		  assertEquals(f2.derivative().applyAsDouble(x), F1.derivative().applyAsDouble(x), 0);
		}
	  }
	}

}