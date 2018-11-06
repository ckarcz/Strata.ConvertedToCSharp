using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.distribution
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MersenneTwister64 = com.opengamma.strata.math.impl.cern.MersenneTwister64;
	using Normal = com.opengamma.strata.math.impl.cern.Normal;
	using Probability = com.opengamma.strata.math.impl.cern.Probability;
	using RandomEngine = com.opengamma.strata.math.impl.cern.RandomEngine;

	/// <summary>
	/// The normal distribution is a continuous probability distribution with probability density function
	/// $$
	/// \begin{align*}
	/// f(x) = \frac{1}{\sqrt{2\pi}\sigma} e^{-\frac{(x - \mu)^2}{2\sigma^2}}
	/// \end{align*}
	/// $$
	/// where $\mu$ is the mean and $\sigma$ the standard deviation of
	/// the distribution.
	/// </summary>
	public class NormalDistribution : ProbabilityDistribution<double>
	{

	  private static readonly double ROOT2 = Math.Sqrt(2);

	  // TODO need a better seed
	  private readonly double _mean;
	  private readonly double _standardDeviation;
	  private readonly Normal _normal;

	  /// <param name="mean"> The mean of the distribution </param>
	  /// <param name="standardDeviation"> The standard deviation of the distribution, not negative or zero </param>
	  public NormalDistribution(double mean, double standardDeviation) : this(mean, standardDeviation, new MersenneTwister64(DateTime.Now))
	  {
	  }

	  /// <param name="mean"> The mean of the distribution </param>
	  /// <param name="standardDeviation"> The standard deviation of the distribution, not negative or zero </param>
	  /// <param name="randomEngine"> A generator of uniform random numbers, not null </param>
	  public NormalDistribution(double mean, double standardDeviation, RandomEngine randomEngine)
	  {
		ArgChecker.isTrue(standardDeviation > 0, "standard deviation");
		ArgChecker.notNull(randomEngine, "randomEngine");
		_mean = mean;
		_standardDeviation = standardDeviation;
		_normal = new Normal(mean, standardDeviation, randomEngine);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double getCDF(double? x)
	  {
		ArgChecker.notNull(x, "x");
		return DERFC.getErfc(-x / ROOT2) / 2;
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double getPDF(double? x)
	  {
		ArgChecker.notNull(x, "x");
		return _normal.pdf(x.Value);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double nextRandom()
	  {
		return _normal.NextDouble();
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double getInverseCDF(double? p)
	  {
		ArgChecker.notNull(p, "p");
		ArgChecker.isTrue(p >= 0 && p <= 1, "Probability must be >= 0 and <= 1");
		return Probability.normalInverse(p.Value);
	  }

	  /// <returns> The mean </returns>
	  public virtual double Mean
	  {
		  get
		  {
			return _mean;
		  }
	  }

	  /// <returns> The standard deviation </returns>
	  public virtual double StandardDeviation
	  {
		  get
		  {
			return _standardDeviation;
		  }
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		long temp;
		temp = System.BitConverter.DoubleToInt64Bits(_mean);
		result = prime * result + (int)(temp ^ ((long)((ulong)temp >> 32)));
		temp = System.BitConverter.DoubleToInt64Bits(_standardDeviation);
		result = prime * result + (int)(temp ^ ((long)((ulong)temp >> 32)));
		return result;
	  }

	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj == null)
		{
		  return false;
		}
		if (this.GetType() != obj.GetType())
		{
		  return false;
		}
		NormalDistribution other = (NormalDistribution) obj;
		if (System.BitConverter.DoubleToInt64Bits(_mean) != Double.doubleToLongBits(other._mean))
		{
		  return false;
		}
		return System.BitConverter.DoubleToInt64Bits(_standardDeviation) == Double.doubleToLongBits(other._standardDeviation);
	  }

	}

}