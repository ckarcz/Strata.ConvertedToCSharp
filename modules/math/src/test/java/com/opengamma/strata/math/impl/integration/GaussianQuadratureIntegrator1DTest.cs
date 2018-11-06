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
//ORIGINAL LINE: @Test public class GaussianQuadratureIntegrator1DTest
	public class GaussianQuadratureIntegrator1DTest
	{

	  private static readonly System.Func<double, double> ONE = (final double? x) =>
	  {

  return 1.0;
	  };

	  private static readonly System.Func<double, double> DF1 = (final double? x) =>
	  {

  return x * x * x * (x - 4);

	  };
	  private static readonly System.Func<double, double> F1 = (final double? x) =>
	  {

  return x * x * x * x * (x / 5.0 - 1);

	  };
	  private static readonly System.Func<double, double> DF2 = (final double? x) =>
	  {

  return Math.Exp(-2 * x);

	  };

	  private static readonly System.Func<double, double> DF3 = (final double? x) =>
	  {

  return Math.Exp(-x * x);

	  };

	  private static readonly System.Func<double, double> COS = (final double? x) =>
	  {
  return Math.Cos(x);
	  };

	  private static readonly System.Func<double, double> COS_EXP = (final double? x) =>
	  {
  return Math.Cos(x) * Math.Exp(-x * x);
	  };

	  private const double EPS = 1e-6;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testGaussLegendre()
	  public virtual void testGaussLegendre()
	  {
		double upper = 2;
		double lower = -6;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Integrator1D<double, double> integrator = new GaussLegendreQuadratureIntegrator1D(6);
		Integrator1D<double, double> integrator = new GaussLegendreQuadratureIntegrator1D(6);
		assertEquals(F1.apply(upper) - F1.apply(lower), integrator.integrate(DF1, lower, upper), EPS);
		lower = -0.56;
		upper = 1.4;
		assertEquals(F1.apply(upper) - F1.apply(lower), integrator.integrate(DF1, lower, upper), EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testGaussLaguerre()
	  public virtual void testGaussLaguerre()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double upper = Double.POSITIVE_INFINITY;
		double upper = double.PositiveInfinity;
		const double lower = 0;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Integrator1D<double, double> integrator = new GaussLaguerreQuadratureIntegrator1D(15);
		Integrator1D<double, double> integrator = new GaussLaguerreQuadratureIntegrator1D(15);
		assertEquals(0.5, integrator.integrate(DF2, lower, upper), EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testRungeKutta()
	  public virtual void testRungeKutta()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final RungeKuttaIntegrator1D integrator = new RungeKuttaIntegrator1D();
		RungeKuttaIntegrator1D integrator = new RungeKuttaIntegrator1D();
		const double lower = -1;
		const double upper = 2;
		assertEquals(F1.apply(upper) - F1.apply(lower), integrator.integrate(DF1, lower, upper), EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testGaussJacobi()
	  public virtual void testGaussJacobi()
	  {
		const double upper = 12;
		const double lower = -1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Integrator1D<double, double> integrator = new GaussJacobiQuadratureIntegrator1D(7);
		Integrator1D<double, double> integrator = new GaussJacobiQuadratureIntegrator1D(7);
		assertEquals(F1.apply(upper) - F1.apply(lower), integrator.integrate(DF1, lower, upper), EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testGaussHermite()
	  public virtual void testGaussHermite()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double rootPI = Math.sqrt(Math.PI);
		double rootPI = Math.Sqrt(Math.PI);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double upper = Double.POSITIVE_INFINITY;
		double upper = double.PositiveInfinity;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double lower = Double.NEGATIVE_INFINITY;
		double lower = double.NegativeInfinity;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GaussHermiteQuadratureIntegrator1D integrator = new GaussHermiteQuadratureIntegrator1D(10);
		GaussHermiteQuadratureIntegrator1D integrator = new GaussHermiteQuadratureIntegrator1D(10);
		assertEquals(rootPI, integrator.integrateFromPolyFunc(ONE), 1e-15);
		assertEquals(rootPI, integrator.integrate(DF3, lower, upper), EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testGaussHermite2()
	  public virtual void testGaussHermite2()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final RungeKuttaIntegrator1D rk = new RungeKuttaIntegrator1D(1e-15);
		RungeKuttaIntegrator1D rk = new RungeKuttaIntegrator1D(1e-15);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final System.Nullable<double> expected = 2 * rk.integrate(COS_EXP, 0.0, 10.0);
		double? expected = 2 * rk.integrate(COS_EXP, 0.0, 10.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final GaussHermiteQuadratureIntegrator1D gh = new GaussHermiteQuadratureIntegrator1D(11);
		GaussHermiteQuadratureIntegrator1D gh = new GaussHermiteQuadratureIntegrator1D(11);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double res1 = gh.integrateFromPolyFunc(COS);
		double res1 = gh.integrateFromPolyFunc(COS);
		assertEquals(expected, res1, 1e-15); //11 points gets you machine precision
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double res2 = gh.integrate(COS_EXP, Double.NEGATIVE_INFINITY, Double.POSITIVE_INFINITY);
		double res2 = gh.integrate(COS_EXP, double.NegativeInfinity, double.PositiveInfinity).Value;
		assertEquals(expected, res2, 1e-15);
	  }

	}

}