/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Assert = org.testng.Assert;
	using Test = org.testng.annotations.Test;

	using RealPolynomialFunction1D = com.opengamma.strata.math.impl.function.RealPolynomialFunction1D;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class QuadraticRealRootFinderTest
	public class QuadraticRealRootFinderTest
	{
	  private const double EPS = 1e-9;
	  private static readonly RealPolynomialFunction1D F = new RealPolynomialFunction1D(12.0, 7.0, 1.0);
	  private static readonly Polynomial1DRootFinder<double> FINDER = new QuadraticRealRootFinder();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		try
		{
		  FINDER.getRoots(null);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		try
		{
		  FINDER.getRoots(new RealPolynomialFunction1D(1.0, 2.0, 3.0, 4.0));
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		try
		{
		  FINDER.getRoots(new RealPolynomialFunction1D(12.0, 1.0, 12.0));
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final com.opengamma.strata.math.MathException e)
		catch (m.opengamma.strata.math.MathException)
		{
		  // Expected
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final System.Nullable<double>[] roots = FINDER.getRoots(F);
		double?[] roots = FINDER.getRoots(F);
		assertEquals(roots[0], -4.0, EPS);
		assertEquals(roots[1], -3.0, EPS);
	  }
	}

}