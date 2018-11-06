using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function.special
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IncompleteGammaFunctionTest
	public class IncompleteGammaFunctionTest
	{
	  private const double A = 1;
	  private static readonly System.Func<double, double> FUNCTION = new IncompleteGammaFunction(A);
	  private const double EPS = 1e-9;
	  private const int MAX_ITER = 10000;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeA1()
	  public virtual void testNegativeA1()
	  {
		new IncompleteGammaFunction(-A);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeA2()
	  public virtual void testNegativeA2()
	  {
		new IncompleteGammaFunction(-A, MAX_ITER, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeIter()
	  public virtual void testNegativeIter()
	  {
		new IncompleteGammaFunction(A, -MAX_ITER, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeEps()
	  public virtual void testNegativeEps()
	  {
		new IncompleteGammaFunction(A, MAX_ITER, -EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testLimits()
	  public virtual void testLimits()
	  {
		assertEquals(FUNCTION.apply(0.0), 0, EPS);
		assertEquals(FUNCTION.apply(100.0), 1, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> f = new java.util.function.Function<double, double>()
		System.Func<double, double> f = (final double? x) =>
		{

	return 1 - Math.Exp(-x);

		};
		const double x = 4.6;
		assertEquals(f(x), FUNCTION.apply(x), EPS);
	  }
	}

}