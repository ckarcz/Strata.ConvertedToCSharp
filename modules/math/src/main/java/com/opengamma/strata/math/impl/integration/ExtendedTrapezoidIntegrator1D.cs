using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{

	using TrapezoidIntegrator = org.apache.commons.math3.analysis.integration.TrapezoidIntegrator;
	using UnivariateIntegrator = org.apache.commons.math3.analysis.integration.UnivariateIntegrator;
	using MathIllegalArgumentException = org.apache.commons.math3.exception.MathIllegalArgumentException;
	using MaxCountExceededException = org.apache.commons.math3.exception.MaxCountExceededException;
	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using CommonsMathWrapper = com.opengamma.strata.math.impl.util.CommonsMathWrapper;

	/// <summary>
	/// The trapezoid integration rule is a two-point Newton-Cotes formula that
	/// approximates the area under the curve as a trapezoid. For a function $f(x)$,
	/// $$
	/// \begin{align*}
	/// \int^{x_2} _{x_1} f(x)dx \approx \frac{1}{2}(x_2 - x_1)(f(x_1) + f(x_2))
	/// \end{align*}
	/// $$
	/// <para> 
	/// This class is a wrapper for the
	/// <a href="http://commons.apache.org/proper/commons-math/apidocs/org/apache/commons/math3/analysis/integration/TrapezoidIntegrator.html">Commons Math library implementation</a> 
	/// of trapezoidal integration.
	/// </para>
	/// </summary>
	public class ExtendedTrapezoidIntegrator1D : Integrator1D<double, double>
	{

	  private static readonly Logger log = LoggerFactory.getLogger(typeof(ExtendedTrapezoidIntegrator1D));
	  private static readonly UnivariateIntegrator INTEGRATOR = new TrapezoidIntegrator();
	  private const int MAX_EVAL = 10000;

	  /// <summary>
	  /// Trapezoid integration method. Note that the Commons implementation fails if the lower bound is larger than the upper - 
	  /// in this case, the bounds are reversed and the result negated. 
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double? integrate(System.Func<double, double> f, double? lower, double? upper)
	  {
		ArgChecker.notNull(f, "f");
		ArgChecker.notNull(lower, "lower");
		ArgChecker.notNull(upper, "upper");
		try
		{
		  if (lower < upper)
		  {
			return INTEGRATOR.integrate(MAX_EVAL, CommonsMathWrapper.wrapUnivariate(f), lower, upper);
		  }
		  log.info("Upper bound was less than lower bound; swapping bounds and negating result");
		  return -INTEGRATOR.integrate(MAX_EVAL, CommonsMathWrapper.wrapUnivariate(f), upper, lower);
		}
		catch (Exception e) when (e is MaxCountExceededException || e is MathIllegalArgumentException)
		{
		  throw new MathException(e);
		}
	  }

	}

}