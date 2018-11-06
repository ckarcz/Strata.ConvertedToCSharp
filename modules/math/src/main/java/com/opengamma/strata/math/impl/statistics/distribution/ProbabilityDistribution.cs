/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.distribution
{
	/// <summary>
	/// Interface for probability distributions. </summary>
	/// @param <T> Type of the parameters of the distribution </param>
	// CSOFF: ALL
	public interface ProbabilityDistribution<T>
	{

	  /// <returns> The next random number from this distribution </returns>
	  double nextRandom();

	  /// <summary>
	  /// Return the probability density function for a value </summary>
	  /// <param name="x"> The value, not null </param>
	  /// <returns> The pdf </returns>
	  double getPDF(T x);

	  /// <summary>
	  /// Returns the cumulative distribution function for a value </summary>
	  /// <param name="x"> The value, not null </param>
	  /// <returns> The cdf </returns>
	  double getCDF(T x);

	  /// <summary>
	  /// Given a probability, return the value that returns this cdf </summary>
	  /// <param name="p"> The probability, not null. $0 \geq p \geq 1$ </param>
	  /// <returns> The inverse cdf </returns>
	  double getInverseCDF(T p);

	}

}