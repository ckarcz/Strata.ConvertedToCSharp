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
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RungeKuttaIntegrator1DTest
	public class RungeKuttaIntegrator1DTest
	{

	  private static readonly double ROOT_2PI = Math.Sqrt(2.0 * Math.PI);

	  private static readonly System.Func<double, double> CUBE = (final double? x) =>
	  {

  return x * x * x;

	  };

	  private static readonly System.Func<double, double> TRIANGLE = (final double? x) =>
	  {

  if (x > 1.0 || x < 0.0)
  {
	return x - Math.Floor(x);
  }

  return x;


	  };

	  private static readonly System.Func<double, double> MIX_NORM = new FuncAnonymousInnerClass();

	  private class FuncAnonymousInnerClass : System.Func<double, double>
	  {
		  public FuncAnonymousInnerClass()
		  {
		  }

		  private readonly double[] W = new double[] {0.2, 0.2, 0.2, 0.2, 0.2};
		  private readonly double[] MU = new double[] {0.0, -0.4, 0.5, 0.0, 0.01234583};
		  private readonly double[] SIGMA = new double[] {3.0, 0.1, 5.0, 0.001, 0.0001};

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public System.Nullable<double> apply(final System.Nullable<double> x)
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
		  public override double? apply(double? x)
		  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int n = W.length;
			int n = W.length;
			double res = 0.0;
			double expo;
			for (int i = 0; i < n; i++)
			{
			  expo = (x - MU[i]) * (x - MU[i]) / SIGMA[i] / SIGMA[i];
			  res += W[i] * Math.Exp(-0.5 * expo) / ROOT_2PI / SIGMA[i];
			}
			return res;
		  }
	  }

	  private static readonly System.Func<double, double> SIN_INV_X = (final double? x) =>
	  {
  const double eps = 1e-127;
  if (Math.Abs(x) < eps)
  {
	return 0.0;
  }
  return Math.Sin(1.0 / x);


	  };

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeAbsTol()
	  public virtual void testNegativeAbsTol()
	  {
		new RungeKuttaIntegrator1D(-1.0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeRelTol()
	  public virtual void testNegativeRelTol()
	  {
		new RungeKuttaIntegrator1D(1e-7, -1.0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testLessTahnOneStep()
	  public virtual void testLessTahnOneStep()
	  {
		new RungeKuttaIntegrator1D(0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		const double eps = 1e-9;
		const int minSteps = 10;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Integrator1D<double, double> integrator = new RungeKuttaIntegrator1D(eps, eps, minSteps);
		Integrator1D<double, double> integrator = new RungeKuttaIntegrator1D(eps, eps, minSteps);

		double lower = 0;
		double upper = 2.0;
		assertEquals(4.0, integrator.integrate(CUBE, lower, upper), eps);

		lower = 0.0;
		upper = 1.5;
		assertEquals(0.625, integrator.integrate(TRIANGLE, lower, upper), eps);

		lower = -30;
		upper = 30;
		assertEquals(1.0, integrator.integrate(MIX_NORM, lower, upper), eps);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testCutoff()
	  public virtual void testCutoff()
	  {

		const double eps = 1e-9;
		const int minSteps = 10;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Integrator1D<double, double> integrator = new RungeKuttaIntegrator1D(eps, eps, minSteps);
		Integrator1D<double, double> integrator = new RungeKuttaIntegrator1D(eps, eps, minSteps);
		const double lower = -1.0;
		const double upper = 1.0;
		assertEquals(0.0, integrator.integrate(SIN_INV_X, lower, upper), eps);

	  }

	}

}