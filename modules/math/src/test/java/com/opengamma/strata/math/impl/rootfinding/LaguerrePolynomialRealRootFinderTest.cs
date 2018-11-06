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
//ORIGINAL LINE: @Test public class LaguerrePolynomialRealRootFinderTest
	public class LaguerrePolynomialRealRootFinderTest
	{
	  private const double EPS = 1e-12;
	  private static readonly LaguerrePolynomialRealRootFinder ROOT_FINDER = new LaguerrePolynomialRealRootFinder();
	  private static readonly RealPolynomialFunction1D TWO_REAL_ROOTS = new RealPolynomialFunction1D(12, 7, 1);
	  private static readonly RealPolynomialFunction1D ONE_REAL_ROOT = new RealPolynomialFunction1D(9, -6, 1);
	  private static readonly RealPolynomialFunction1D CLOSE_ROOTS = new RealPolynomialFunction1D(9 + 3 * 1e-6, -6 - 1e-6, 1);
	  private static readonly RealPolynomialFunction1D NO_REAL_ROOTS = new RealPolynomialFunction1D(12, 0, 1);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction()
	  public virtual void testNullFunction()
	  {
		ROOT_FINDER.getRoots(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = com.opengamma.strata.math.MathException.class) public void testNoRealRoots()
	  public virtual void testNoRealRoots()
	  {
		ROOT_FINDER.getRoots(NO_REAL_ROOTS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		double?[] result = ROOT_FINDER.getRoots(TWO_REAL_ROOTS);
		Arrays.sort(result);
		assertEquals(result.Length, 2);
		assertEquals(result[0], -4, EPS);
		assertEquals(result[1], -3, EPS);
		result = ROOT_FINDER.getRoots(ONE_REAL_ROOT);
		assertEquals(result.Length, 2);
		assertEquals(result[0], 3, EPS);
		assertEquals(result[1], 3, EPS);
		result = ROOT_FINDER.getRoots(CLOSE_ROOTS);
		Arrays.sort(result);
		assertEquals(result.Length, 2);
		assertEquals(result[1] - result[0], 1e-6, 1e-8);
	  }
	}

}