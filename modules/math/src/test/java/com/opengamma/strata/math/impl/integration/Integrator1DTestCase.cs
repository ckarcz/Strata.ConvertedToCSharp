using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Abstract test.
	/// </summary>
	public abstract class Integrator1DTestCase
	{
	  private static readonly System.Func<double, double> DF = (final double? x) =>
	  {

  return 1 + Math.Exp(-x);

	  };
	  private static readonly System.Func<double, double> F = (final double? x) =>
	  {

  return x - Math.Exp(-x);

	  };
	  private const double? LOWER = 0.0;
	  private const double? UPPER = 12.0;
	  private const double EPS = 1e-5;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction()
	  public virtual void testNullFunction()
	  {
		Integrator.integrate(null, LOWER, UPPER);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullLowerBound()
	  public virtual void testNullLowerBound()
	  {
		Integrator.integrate(DF, null, UPPER);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullUpperBound()
	  public virtual void testNullUpperBound()
	  {
		Integrator.integrate(DF, LOWER, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertEquals(Integrator.integrate(DF, LOWER, UPPER), F.apply(UPPER) - F.apply(LOWER), EPS);
		assertEquals(Integrator.integrate(DF, UPPER, LOWER), -Integrator.integrate(DF, LOWER, UPPER), EPS);
	  }

	  protected internal abstract Integrator1D<double, double> Integrator {get;}

	}

}