using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.differentiation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ScalarFirstOrderDifferentiatorTest
	public class ScalarFirstOrderDifferentiatorTest
	{
	  private static readonly System.Func<double, double> F = (final double? x) =>
	  {

  return 3 * x * x + 4 * x - Math.Sin(x);
	  };

	  private static readonly System.Func<double, bool> DOMAIN = (final double? x) =>
	  {
  return x >= 0 && x <= Math.PI;
	  };

	  private static readonly System.Func<double, double> DX_ANALYTIC = (final double? x) =>
	  {

  return 6 * x + 4 - Math.Cos(x);

	  };
	  private const double EPS = 1e-5;
	  private static readonly ScalarFirstOrderDifferentiator FORWARD = new ScalarFirstOrderDifferentiator(FiniteDifferenceType.FORWARD, EPS);
	  private static readonly ScalarFirstOrderDifferentiator CENTRAL = new ScalarFirstOrderDifferentiator(FiniteDifferenceType.CENTRAL, EPS);
	  private static readonly ScalarFirstOrderDifferentiator BACKWARD = new ScalarFirstOrderDifferentiator(FiniteDifferenceType.BACKWARD, EPS);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullDifferenceType()
	  public virtual void testNullDifferenceType()
	  {
		new ScalarFirstOrderDifferentiator(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction()
	  public virtual void testNullFunction()
	  {
		CENTRAL.differentiate((System.Func<double, double>) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		const double x = 0.2245;
		assertEquals(FORWARD.differentiate(F).apply(x), DX_ANALYTIC.apply(x), 10 * EPS);
		assertEquals(CENTRAL.differentiate(F).apply(x), DX_ANALYTIC.apply(x), EPS * EPS); // This is why you use central difference
		assertEquals(BACKWARD.differentiate(F).apply(x), DX_ANALYTIC.apply(x), 10 * EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void domainTest()
	  public virtual void domainTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] x = new double[] {1.2, 0, Math.PI };
		double[] x = new double[] {1.2, 0, Math.PI};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> alFunc = CENTRAL.differentiate(F, DOMAIN);
		System.Func<double, double> alFunc = CENTRAL.differentiate(F, DOMAIN);
		for (int i = 0; i < 3; i++)
		{
		  assertEquals(alFunc(x[i]), DX_ANALYTIC.apply(x[i]), 1e-8);
		}
	  }
	}

}