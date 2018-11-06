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

	using RealPolynomialFunction1D = com.opengamma.strata.math.impl.function.RealPolynomialFunction1D;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CubicRootFindingTest
	public class CubicRootFindingTest
	{
	  private static readonly CubicRootFinder CUBIC = new CubicRootFinder();
	  private static readonly CubicRealRootFinder REAL_ONLY_CUBIC = new CubicRealRootFinder();
	  private static readonly RealPolynomialFunction1D ONE_REAL_ROOT = new RealPolynomialFunction1D(-10, 10, -3, 3);
	  private static readonly RealPolynomialFunction1D ONE_DISTINCT_ROOT = new RealPolynomialFunction1D(-1, 3, -3, 1);
	  private static readonly RealPolynomialFunction1D THREE_ROOTS = new RealPolynomialFunction1D(-6, 11, -6, 1);
	  private const double EPS = 1e-12;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction1()
	  public virtual void testNullFunction1()
	  {
		CUBIC.getRoots(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNonCubic1()
	  public virtual void testNonCubic1()
	  {
		CUBIC.getRoots(new RealPolynomialFunction1D(1, 1, 1, 1, 1));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction2()
	  public virtual void testNullFunction2()
	  {
		REAL_ONLY_CUBIC.getRoots(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNonCubic2()
	  public virtual void testNonCubic2()
	  {
		REAL_ONLY_CUBIC.getRoots(new RealPolynomialFunction1D(1, 1, 1, 1, 1));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testCubic()
	  public virtual void testCubic()
	  {
		ComplexNumber[] result = CUBIC.getRoots(ONE_REAL_ROOT);
		assertEquals(result.Length, 3);
		assertComplexEquals(result[0], new ComplexNumber(1, 0));
		assertComplexEquals(result[1], new ComplexNumber(0, Math.Sqrt(10 / 3.0)));
		assertComplexEquals(result[2], new ComplexNumber(0, -Math.Sqrt(10 / 3.0)));
		result = CUBIC.getRoots(ONE_DISTINCT_ROOT);
		assertEquals(result.Length, 3);
		foreach (ComplexNumber c in result)
		{
		  assertComplexEquals(c, new ComplexNumber(1, 0));
		}
		result = CUBIC.getRoots(THREE_ROOTS);
		assertEquals(result.Length, 3);
		assertComplexEquals(result[0], new ComplexNumber(1, 0));
		assertComplexEquals(result[1], new ComplexNumber(3, 0));
		assertComplexEquals(result[2], new ComplexNumber(2, 0));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testRealOnlyCubic()
	  public virtual void testRealOnlyCubic()
	  {
		double?[] result = REAL_ONLY_CUBIC.getRoots(ONE_REAL_ROOT);
		assertEquals(result.Length, 1);
		assertEquals(result[0], 1, 0);
		result = REAL_ONLY_CUBIC.getRoots(ONE_DISTINCT_ROOT);
		assertEquals(result.Length, 3);
		foreach (double? d in result)
		{
		  assertEquals(d, 1, EPS);
		}
		result = REAL_ONLY_CUBIC.getRoots(THREE_ROOTS);
		assertEquals(result.Length, 3);
		assertEquals(result[0], 1, EPS);
		assertEquals(result[1], 3, EPS);
		assertEquals(result[2], 2, EPS);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private void assertComplexEquals(final com.opengamma.strata.math.impl.ComplexNumber c1, final com.opengamma.strata.math.impl.ComplexNumber c2)
	  private void assertComplexEquals(ComplexNumber c1, ComplexNumber c2)
	  {
		assertEquals(c1.Real, c2.Real, EPS);
		assertEquals(c1.Imaginary, c2.Imaginary, EPS);
	  }
	}

}