using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{

	using RombergIntegrator = org.apache.commons.math3.analysis.integration.RombergIntegrator;
	using UnivariateIntegrator = org.apache.commons.math3.analysis.integration.UnivariateIntegrator;
	using MathIllegalArgumentException = org.apache.commons.math3.exception.MathIllegalArgumentException;
	using MaxCountExceededException = org.apache.commons.math3.exception.MaxCountExceededException;
	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using CommonsMathWrapper = com.opengamma.strata.math.impl.util.CommonsMathWrapper;

	/// 
	/// <summary>
	/// Romberg's method estimates an integral by repeatedly using <a href="http://en.wikipedia.org/wiki/Richardson_extrapolation">Richardson extrapolation</a> 
	/// on the extended trapezium rule <seealso cref="ExtendedTrapezoidIntegrator1D"/>. 
	/// <para>
	/// This class is a wrapper for the
	/// <a href="http://commons.apache.org/proper/commons-math/apidocs/org/apache/commons/math3/analysis/integration/RombergIntegrator.html">Commons Math library implementation</a> 
	/// of Romberg integration.
	/// </para>
	/// </summary>
	public class RombergIntegrator1D : Integrator1D<double, double>
	{

	  private static readonly Logger log = LoggerFactory.getLogger(typeof(RombergIntegrator1D));
	  private const int MAX_EVAL = 10000;
	  private readonly UnivariateIntegrator integrator = new RombergIntegrator();

	  /// <summary>
	  /// Romberg integration method. Note that the Commons implementation fails if the lower bound is larger than the upper - 
	  /// in this case, the bounds are reversed and the result negated. </summary>
	  /// <param name="f"> The function to integrate, not null </param>
	  /// <param name="lower"> The lower bound, not null </param>
	  /// <param name="upper"> The upper bound, not null </param>
	  /// <returns> The result of the integration </returns>
	  public virtual double? integrate(System.Func<double, double> f, double? lower, double? upper)
	  {
		ArgChecker.notNull(f, "f");
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
		catch (Exception e) when (e is MaxCountExceededException || e is MathIllegalArgumentException)
		{
		  throw new MathException(e);
		}
	  }

	}

}