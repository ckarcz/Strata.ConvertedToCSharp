using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleFunction1D = com.opengamma.strata.math.impl.function.DoubleFunction1D;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NewtonRaphsonSingleRootFinderTest
	public class NewtonRaphsonSingleRootFinderTest
	{
	  private static readonly DoubleFunction1D F1 = new DoubleFunction1DAnonymousInnerClass();

	  private class DoubleFunction1DAnonymousInnerClass : DoubleFunction1D
	  {
		  public DoubleFunction1DAnonymousInnerClass()
		  {
		  }


		  public override double applyAsDouble(double x)
		  {
			return x * x * x - 6 * x * x + 11 * x - 106;
		  }

		  public override DoubleFunction1D derivative()
		  {
			return x => 3 * x * x - 12 * x + 11;
		  }

	  }
	  private static readonly System.Func<double, double> F2 = (final double? x) =>
	  {

  return x * x * x - 6 * x * x + 11 * x - 106;

	  };
	  private static readonly DoubleFunction1D DF1 = x => 3 * x * x - 12 * x + 11;
	  private static readonly System.Func<double, double> DF2 = (final double? x) =>
	  {

  return 3 * x * x - 12 * x + 11;

	  };
	  private static readonly NewtonRaphsonSingleRootFinder ROOT_FINDER = new NewtonRaphsonSingleRootFinder();
	  private const double X1 = 4;
	  private const double X2 = 10;
	  private const double X3 = -10;
	  private const double X = 6;
	  private static readonly double ROOT;
	  private const double EPS = 1e-12;

	  static NewtonRaphsonSingleRootFinderTest()
	  {
		const double q = 1.0 / 3;
		const double r = -50;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double a = Math.pow(Math.abs(r) + Math.sqrt(r * r - q * q * q), 1.0 / 3);
		double a = Math.Pow(Math.Abs(r) + Math.Sqrt(r * r - q * q * q), 1.0 / 3);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double b = q / a;
		double b = q / a;
		ROOT = a + b + 2;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction1()
	  public virtual void testNullFunction1()
	  {
		ROOT_FINDER.getRoot((System.Func<double, double>) null, X1, X2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullLower1()
	  public virtual void testNullLower1()
	  {
		ROOT_FINDER.getRoot(F2, (double?) null, X2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullHigher1()
	  public virtual void testNullHigher1()
	  {
		ROOT_FINDER.getRoot(F2, X1, (double?) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction2()
	  public virtual void testNullFunction2()
	  {
		ROOT_FINDER.getRoot((DoubleFunction1D) null, X1, X2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullLower2()
	  public virtual void testNullLower2()
	  {
		ROOT_FINDER.getRoot(F1, (double?) null, X2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullHigher2()
	  public virtual void testNullHigher2()
	  {
		ROOT_FINDER.getRoot(F1, X1, (double?) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction3()
	  public virtual void testNullFunction3()
	  {
		ROOT_FINDER.getRoot(null, DF2, X1, X2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullDerivative1()
	  public virtual void testNullDerivative1()
	  {
		ROOT_FINDER.getRoot(F2, null, X1, X2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullLower3()
	  public virtual void testNullLower3()
	  {
		ROOT_FINDER.getRoot(F2, DF2, null, X2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullHigher3()
	  public virtual void testNullHigher3()
	  {
		ROOT_FINDER.getRoot(F2, DF2, X1, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction4()
	  public virtual void testNullFunction4()
	  {
		ROOT_FINDER.getRoot(null, DF1, X1, X2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullDerivative2()
	  public virtual void testNullDerivative2()
	  {
		ROOT_FINDER.getRoot(F1, null, X1, X2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullLower4()
	  public virtual void testNullLower4()
	  {
		ROOT_FINDER.getRoot(F1, DF1, null, X2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullHigher4()
	  public virtual void testNullHigher4()
	  {
		ROOT_FINDER.getRoot(F1, DF1, X1, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testEnclosedExtremum()
	  public virtual void testEnclosedExtremum()
	  {
		ROOT_FINDER.getRoot(F2, DF2, X1, X3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullDerivative3()
	  public virtual void testNullDerivative3()
	  {
		ROOT_FINDER.getRoot(F1, (DoubleFunction1D) null, X);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullDerivative4()
	  public virtual void testNullDerivative4()
	  {
		ROOT_FINDER.getRoot(F2, (System.Func<double, double>) null, X);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction5()
	  public virtual void testNullFunction5()
	  {
		ROOT_FINDER.getRoot((System.Func<double, double>) null, X);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction6()
	  public virtual void testNullFunction6()
	  {
		ROOT_FINDER.getRoot((DoubleFunction1D) null, X);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullInitialGuess1()
	  public virtual void testNullInitialGuess1()
	  {
		ROOT_FINDER.getRoot(F1, (double?) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullInitialGuess2()
	  public virtual void testNullInitialGuess2()
	  {
		ROOT_FINDER.getRoot(F2, (double?) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullInitialGuess3()
	  public virtual void testNullInitialGuess3()
	  {
		ROOT_FINDER.getRoot(F1, DF1, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullInitialGuess4()
	  public virtual void testNullInitialGuess4()
	  {
		ROOT_FINDER.getRoot(F2, DF2, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertEquals(ROOT_FINDER.getRoot(F2, DF2, ROOT, X2), ROOT, 0);
		assertEquals(ROOT_FINDER.getRoot(F2, DF2, X1, ROOT), ROOT, 0);
		assertEquals(ROOT_FINDER.getRoot(F1, X1, X2), ROOT, EPS);
		assertEquals(ROOT_FINDER.getRoot(F1, DF1, X1, X2), ROOT, EPS);
		assertEquals(ROOT_FINDER.getRoot(F2, X1, X2), ROOT, EPS);
		assertEquals(ROOT_FINDER.getRoot(F2, DF2, X1, X2), ROOT, EPS);
		assertEquals(ROOT_FINDER.getRoot(F1, X), ROOT, EPS);
		assertEquals(ROOT_FINDER.getRoot(F1, DF1, X), ROOT, EPS);
		assertEquals(ROOT_FINDER.getRoot(F2, X), ROOT, EPS);
		assertEquals(ROOT_FINDER.getRoot(F2, DF2, X), ROOT, EPS);
	  }
	}

}