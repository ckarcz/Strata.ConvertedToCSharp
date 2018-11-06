using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{

	using SimpsonIntegrator = org.apache.commons.math3.analysis.integration.SimpsonIntegrator;
	using UnivariateIntegrator = org.apache.commons.math3.analysis.integration.UnivariateIntegrator;
	using NumberIsTooLargeException = org.apache.commons.math3.exception.NumberIsTooLargeException;
	using NumberIsTooSmallException = org.apache.commons.math3.exception.NumberIsTooSmallException;
	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using CommonsMathWrapper = com.opengamma.strata.math.impl.util.CommonsMathWrapper;

	/// <summary>
	/// Simpson's integration rule is a Newton-Cotes formula that approximates the
	/// function to be integrated with quadratic polynomials before performing the
	/// integration. For a function $f(x)$, if three points $x_1$, $x_2$ and $x_3$
	/// are equally spaced on the abscissa with $x_2 - x_1 = h$ then
	/// $$
	/// \begin{align*}
	/// \int^{x_3} _{x_1} f(x)dx \approx \frac{1}{3}h(f(x_1) + 4f(x_2) + f(x_3))
	/// \end{align*}
	/// $$
	/// <para> 
	/// This class is a wrapper for the
	/// <a href="http://commons.apache.org/proper/commons-math/apidocs/org/apache/commons/math3/analysis/integration/SimpsonIntegrator.html">Commons Math library implementation</a> 
	/// of Simpson integration.
	/// </para>
	/// </summary>
	public class SimpsonIntegrator1D : Integrator1D<double, double>
	{

	  private static readonly Logger log = LoggerFactory.getLogger(typeof(SimpsonIntegrator1D));
	  private const int MAX_EVAL = 1000;
	  private readonly UnivariateIntegrator integrator = new SimpsonIntegrator();

	  /// <summary>
	  /// Simpson's integration method.
	  /// <para>
	  /// Note that the Commons implementation fails if the lower bound is larger than the upper - 
	  /// in this case, the bounds are reversed and the result negated. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="f"> The function to integrate, not null </param>
	  /// <param name="lower"> The lower bound, not null </param>
	  /// <param name="upper"> The upper bound, not null </param>
	  /// <returns> The result of the integration </returns>
	  public virtual double? integrate(System.Func<double, double> f, double? lower, double? upper)
	  {
		ArgChecker.notNull(f, "function");
		ArgChecker.notNull(lower, "lower bound");
		ArgChecker.notNull(upper, "upper bound");
		try
		{
		  if (lower < upper)
		  {
			return integrator.integrate(MAX_EVAL, CommonsMathWrapper.wrapUnivariate(f), lower, upper);
		  }
		  log.info("Upper bound was less than lower bound; swapping bounds and negating result");
		  return -integrator.integrate(MAX_EVAL, CommonsMathWrapper.wrapUnivariate(f), upper, lower);
		}
		catch (Exception e) when (e is NumberIsTooSmallException || e is NumberIsTooLargeException)
		{
		  throw new MathException(e);
		}
	  }

	}

}