using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.distribution
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Gamma = com.opengamma.strata.math.impl.cern.Gamma;
	using MersenneTwister = com.opengamma.strata.math.impl.cern.MersenneTwister;
	using RandomEngine = com.opengamma.strata.math.impl.cern.RandomEngine;

	/// <summary>
	/// The Gamma distribution is a continuous probability distribution with cdf
	/// $$
	/// \begin{align*}
	/// F(x)=\frac{\gamma\left(k, \frac{x}{\theta}\right)}{\Gamma(k)}
	/// \end{align*}
	/// $$
	/// and pdf
	/// $$
	/// \begin{align*}
	/// f(x)=\frac{x^{k-1}e^{-\frac{x}{\theta}}}{\Gamma{k}\theta^k}
	/// \end{align*}
	/// $$
	/// where $k$ is the shape parameter and $\theta$ is the scale parameter.
	/// <para>
	/// </para>
	/// </summary>
	public class GammaDistribution : ProbabilityDistribution<double>
	{

	  private readonly Gamma _gamma;
	  private readonly double _k;
	  private readonly double _theta;

	  /// <param name="k"> The shape parameter of the distribution, not negative or zero </param>
	  /// <param name="theta"> The scale parameter of the distribution, not negative or zero </param>
	  public GammaDistribution(double k, double theta) : this(k, theta, new MersenneTwister(DateTime.Now))
	  {
	  }

	  /// <param name="k"> The shape parameter of the distribution, not negative or zero </param>
	  /// <param name="theta"> The scale parameter of the distribution, not negative or zero </param>
	  /// <param name="engine"> A uniform random number generator, not null </param>
	  public GammaDistribution(double k, double theta, RandomEngine engine)
	  {
		ArgChecker.isTrue(k > 0, "k must be > 0");
		ArgChecker.isTrue(theta > 0, "theta must be > 0");
		ArgChecker.notNull(engine, "engine");
		_gamma = new Gamma(k, 1.0 / theta, engine);
		_k = k;
		_theta = theta;
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double getCDF(double? x)
	  {
		ArgChecker.notNull(x, "x");
		return _gamma.cdf(x.Value);
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <returns> Not supported </returns>
	  /// <exception cref="UnsupportedOperationException"> always </exception>
	  public virtual double getInverseCDF(double? p)
	  {
		throw new System.NotSupportedException();
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double getPDF(double? x)
	  {
		ArgChecker.notNull(x, "x");
		return _gamma.pdf(x.Value);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double nextRandom()
	  {
		return _gamma.NextDouble();
	  }

	  /// <returns> The shape parameter </returns>
	  public virtual double K
	  {
		  get
		  {
			return _k;
		  }
	  }

	  /// <returns> The location parameter </returns>
	  public virtual double Theta
	  {
		  get
		  {
			return _theta;
		  }
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		long temp;
		temp = System.BitConverter.DoubleToInt64Bits(_k);
		result = prime * result + (int)(temp ^ ((long)((ulong)temp >> 32)));
		temp = System.BitConverter.DoubleToInt64Bits(_theta);
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
		GammaDistribution other = (GammaDistribution) obj;
		if (System.BitConverter.DoubleToInt64Bits(_k) != Double.doubleToLongBits(other._k))
		{
		  return false;
		}
		return System.BitConverter.DoubleToInt64Bits(_theta) == Double.doubleToLongBits(other._theta);
	  }

	}

}