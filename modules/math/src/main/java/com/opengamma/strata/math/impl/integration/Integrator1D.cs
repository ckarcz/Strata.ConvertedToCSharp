/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{

	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Class for defining the integration of 1-D functions.
	/// </summary>
	/// @param <T> Type of the function output and result </param>
	/// @param <U> Type of the function inputs and integration bounds </param>
	public abstract class Integrator1D<T, U> : Integrator<T, U, System.Func<U, T>>
	{
		public abstract T integrate(V f, U[] lower, U[] upper);

	  private static readonly Logger log = LoggerFactory.getLogger(typeof(Integrator1D));

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual T integrate(System.Func<U, T> f, U[] lower, U[] upper)
	  {
		ArgChecker.notNull(f, "function was null");
		ArgChecker.notNull(lower, "lower bound array was null");
		ArgChecker.notNull(upper, "upper bound array was null");
		ArgChecker.notEmpty(lower, "lower bound array was empty");
		ArgChecker.notEmpty(upper, "upper bound array was empty");
		ArgChecker.notNull(lower[0], "lower bound was null");
		ArgChecker.notNull(upper[0], "upper bound was null");
		if (lower.Length > 1)
		{
		  log.info("Lower bound array had more than one element; only using the first");
		}
		if (upper.Length > 1)
		{
		  log.info("Upper bound array had more than one element; only using the first");
		}
		return integrate(f, lower[0], upper[0]);
	  }

	  /// <summary>
	  /// 1-D integration method. </summary>
	  /// <param name="f"> The function to integrate, not null </param>
	  /// <param name="lower"> The lower bound, not null </param>
	  /// <param name="upper"> The upper bound, not null </param>
	  /// <returns> The result of the integration </returns>
	  public abstract T integrate(System.Func<U, T> f, U lower, U upper);

	}

}