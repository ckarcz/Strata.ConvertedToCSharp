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
	/// Abstract test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public abstract class RealSingleRootFinderTestCase
	public abstract class RealSingleRootFinderTestCase
	{
	  protected internal static readonly System.Func<double, double> F = (double? x) =>
	  {
  return x * x * x - 4 * x * x + x + 6;
	  };
	  protected internal const double EPS = 1e-9;

	  protected internal abstract RealSingleRootFinder RootFinder {get;}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction()
	  public virtual void testNullFunction()
	  {
		RootFinder.checkInputs((DoubleFunction1D) null, 1.0, 2.0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullLower()
	  public virtual void testNullLower()
	  {
		RootFinder.checkInputs(F, null, 2.0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullUpper()
	  public virtual void testNullUpper()
	  {
		RootFinder.checkInputs(F, 1.0, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOutsideRoots()
	  public virtual void testOutsideRoots()
	  {
		RootFinder.getRoot(F, 10.0, 100.0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testBracketTwoRoots()
	  public virtual void testBracketTwoRoots()
	  {
		RootFinder.getRoot(F, 1.5, 3.5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		RealSingleRootFinder finder = RootFinder;
		assertEquals(finder.getRoot(F, 2.5, 3.5), 3, EPS);
		assertEquals(finder.getRoot(F, 1.5, 2.5), 2, EPS);
		assertEquals(finder.getRoot(F, -1.5, 0.5), -1, EPS);
	  }
	}

}