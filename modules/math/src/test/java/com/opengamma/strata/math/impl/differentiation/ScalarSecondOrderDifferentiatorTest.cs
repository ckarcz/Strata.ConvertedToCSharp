using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.differentiation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="ScalarSecondOrderDifferentiator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ScalarSecondOrderDifferentiatorTest
	public class ScalarSecondOrderDifferentiatorTest
	{

	  private static readonly System.Func<double, double> F = (final double? x) =>
	  {
  return 3d * x * x + 4d * x - Math.Sin(x);
	  };
	  private static readonly System.Func<double, bool> DOMAIN = (final double? x) =>
	  {
  return x >= 0d && x <= Math.PI;
	  };
	  private static readonly System.Func<double, double> DX_ANALYTIC = (final double? x) =>
	  {
  return 6d + Math.Sin(x);
	  };
	  private static readonly ScalarSecondOrderDifferentiator CALC = new ScalarSecondOrderDifferentiator();
	  private const double EPS = 1.0e-4;

	  public virtual void testNullDifferenceType()
	  {
		assertThrowsIllegalArg(() => new ScalarFirstOrderDifferentiator(null));
	  }

	  public virtual void testNullFunction()
	  {
		assertThrowsIllegalArg(() => CALC.differentiate((System.Func<double, double>) null));
	  }

	  public virtual void testDomainOut()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: java.util.function.Function<double, bool> domain = new java.util.function.Function<double, bool>()
		System.Func<double, bool> domain = (double? x) =>
		{
	return x >= 0d && x <= 1.0e-8;
		};
		assertThrowsIllegalArg(() => CALC.differentiate(F, domain).apply(1.0e-9));
	  }

	  public virtual void analyticTest()
	  {
		const double x = 0.2245;
		assertEquals(CALC.differentiate(F).apply(x), DX_ANALYTIC.apply(x), EPS);
	  }

	  public virtual void domainTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] x = new double[] {1.2, 0, Math.PI };
		double[] x = new double[] {1.2, 0, Math.PI};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> alFunc = CALC.differentiate(F, DOMAIN);
		System.Func<double, double> alFunc = CALC.differentiate(F, DOMAIN);
		for (int i = 0; i < 3; i++)
		{
		  assertEquals(alFunc(x[i]), DX_ANALYTIC.apply(x[i]), EPS);
		}
	  }

	}

}