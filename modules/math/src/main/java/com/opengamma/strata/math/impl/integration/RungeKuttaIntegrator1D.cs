using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Adapted from the forth-order Runge-Kutta method for solving ODE. See <a
	/// href="http://en.wikipedia.org/wiki/Runge-Kutta_methods">here </a> for the
	/// maths. It is a very robust integrator and should be used before trying more
	/// specialised methods.
	/// </summary>
	//CSOFF: JavadocMethod
	public class RungeKuttaIntegrator1D : Integrator1D<double, double>
	{

	  private const double DEF_TOL = 1e-10;
	  private const double STEP_SIZE_LIMIT = 1e-50;
	  private const int DEF_MIN_STEPS = 10;
	  private readonly double _absTol, _relTol;
	  private readonly int _minSteps;

	  /// <summary>
	  /// Constructor from absolute and relative tolerance and minimal number of steps.
	  /// <para>
	  /// The adaptable integration process stops when the difference between 2 steps is below the absolute tolerance
	  /// plus the relative tolerance multiplied by the value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="absTol">  the absolute tolerance </param>
	  /// <param name="relTol">  the relative tolerance </param>
	  /// <param name="minSteps">  the minimal number of steps </param>
	  public RungeKuttaIntegrator1D(double absTol, double relTol, int minSteps)
	  {
		if (absTol < 0.0 || double.IsNaN(absTol) || double.IsInfinity(absTol))
		{
		  throw new System.ArgumentException("Absolute Tolerance must be greater than zero");
		}
		if (relTol < 0.0 || double.IsNaN(relTol) || double.IsInfinity(relTol))
		{
		  throw new System.ArgumentException("Relative Tolerance must be greater than zero");
		}
		if (minSteps < 1)
		{
		  throw new System.ArgumentException("Must have minimum of 1 step");
		}
		_absTol = absTol;
		_relTol = relTol;
		_minSteps = minSteps;
	  }

	  public RungeKuttaIntegrator1D(double tol, int minSteps) : this(tol, tol, minSteps)
	  {
	  }
	  public RungeKuttaIntegrator1D(double atol, double rtol) : this(atol, rtol, DEF_MIN_STEPS)
	  {
	  }

	  public RungeKuttaIntegrator1D(double tol) : this(tol, tol, DEF_MIN_STEPS)
	  {
	  }

	  public RungeKuttaIntegrator1D(int minSteps) : this(DEF_TOL, minSteps)
	  {
	  }

	  public RungeKuttaIntegrator1D() : this(DEF_TOL, DEF_MIN_STEPS)
	  {

	  }

	  public virtual double RelativeTolerance
	  {
		  get
		  {
			return _relTol;
		  }
	  }

	  public virtual double? integrate(System.Func<double, double> f, double? lower, double? upper)
	  {
		ArgChecker.notNull(lower, "lower");
		ArgChecker.notNull(upper, "upper");
		if (double.IsNaN(lower) || double.IsInfinity(lower) || double.IsInfinity(upper) || double.IsNaN(upper))
		{
		  throw new System.ArgumentException("lower or upper was NaN or Inf");
		}

		double h = (upper - lower) / _minSteps;
		double f1, f2, f3, x;
		x = lower.Value;
		f1 = f(x);
		if (double.IsNaN(f1) || double.IsInfinity(f1))
		{
		  throw new System.ArgumentException("function evaluation returned NaN or Inf");
		}

		double result = 0.0;
		for (int i = 0; i < _minSteps; i++)
		{
		  f2 = f(x + h / 2.0);
		  if (double.IsNaN(f2) || double.IsInfinity(f2))
		  {
			throw new System.ArgumentException("function evaluation returned NaN or Inf");
		  }
		  f3 = f(x + h);
		  if (double.IsNaN(f3) || double.IsInfinity(f3))
		  {
			throw new System.ArgumentException("function evaluation returned NaN or Inf");
		  }

		  result += calculateRungeKuttaFourthOrder(f, x, h, f1, f2, f3);
		  f1 = f3;
		  x += h;
		}
		return result;
	  }

	  private double calculateRungeKuttaFourthOrder(System.Func<double, double> f, double x, double h, double fl, double fm, double fu)
	  {
		//    if (Double.isNaN(h) || Double.isInfinite(h) || 
		//        Double.isNaN(fl) || Double.isInfinite(fl) ||
		//        Double.isNaN(fm) || Double.isInfinite(fm) ||
		//        Double.isNaN(fu) || Double.isInfinite(fu)) {
		//      throw new OpenGammaRuntimeException("h was Inf or NaN");
		//    }
		double f1 = f(x + 0.25 * h);
		if (double.IsNaN(f1) || double.IsInfinity(f1))
		{
		  throw new System.InvalidOperationException("f.evaluate returned NaN or Inf");
		}
		double f2 = f(x + 0.75 * h);
		if (double.IsNaN(f2) || double.IsInfinity(f2))
		{
		  throw new System.InvalidOperationException("f.evaluate returned NaN or Inf");
		}
		double ya = h * (fl + 4.0 * fm + fu) / 6.0;
		double yb = h * (fl + 2.0 * fm + 4.0 * (f1 + f2) + fu) / 12.0;

		double diff = Math.Abs(ya - yb);
		double abs = Math.Max(Math.Abs(ya), Math.Abs(yb));

		if (diff < _absTol + _relTol * abs)
		{
		  return yb + (yb - ya) / 15.0;
		}

		// can't keep halving the step size
		if (h < STEP_SIZE_LIMIT)
		{
		  return yb + (yb - ya) / 15.0;
		}

		return calculateRungeKuttaFourthOrder(f, x, h / 2.0, fl, f1, fm) + calculateRungeKuttaFourthOrder(f, x + h / 2.0, h / 2.0, fm, f2, fu);
	  }

	}

}