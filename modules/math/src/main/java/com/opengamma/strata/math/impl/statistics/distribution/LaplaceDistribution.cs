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
	using RandomEngine = com.opengamma.strata.math.impl.cern.RandomEngine;

	/// <summary>
	/// The Laplace distribution is a continuous probability distribution with probability density function
	/// $$
	/// \begin{align*}
	/// f(x)=\frac{1}{2b}e^{-\frac{|x-\mu|}{b}}
	/// \end{align*}
	/// $$
	/// where $\mu$ is the location parameter and $b$ is the scale parameter. The
	/// cumulative distribution function and its inverse are defined as:
	/// $$
	/// \begin{align*}
	/// F(x)&=
	/// \begin{cases}
	/// \frac{1}{2}e^{\frac{x-\mu}{b}} & \text{if } x < \mu\\
	/// 1-\frac{1}{2}e^{-\frac{x-\mu}{b}} & \text{if } x\geq \mu
	/// \end{cases}\\
	/// F^{-1}(p)&=\mu-b\text{ sgn}(p-0.5)\ln(1-2|p-0.5|)
	/// \end{align*}
	/// $$
	/// Given a uniform random variable $U$ drawn from the interval $(-\frac{1}{2}, \frac{1}{2}]$,  
	/// a Laplace-distributed random variable with parameters $\mu$ and $b$ is given by:
	/// $$
	/// \begin{align*}
	/// X=\mu-b\text{ sgn}(U)\ln(1-2|U|)
	/// \end{align*}
	/// $$
	/// 
	/// </summary>
	public class LaplaceDistribution : ProbabilityDistribution<double>
	{
	  // TODO need a better seed
	  private readonly RandomEngine _engine;
	  private readonly double _mu;
	  private readonly double _b;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="mu"> The location parameter </param>
	  /// <param name="b"> The scale parameter, greater than zero </param>
	  public LaplaceDistribution(double mu, double b) : this(mu, b, new MersenneTwister64(DateTime.Now))
	  {
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="mu"> The location parameter </param>
	  /// <param name="b"> The scale parameter, greater than zero </param>
	  /// <param name="engine"> A uniform random number generator, not null </param>
	  public LaplaceDistribution(double mu, double b, RandomEngine engine)
	  {
		ArgChecker.isTrue(b > 0, "b must be > 0");
		ArgChecker.notNull(engine, "engine");
		_mu = mu;
		_b = b;
		_engine = engine;
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double getCDF(double? x)
	  {
		ArgChecker.notNull(x, "x");
		return 0.5 * (1 + Math.Sign(x - _mu) * (1 - Math.Exp(-Math.Abs(x - _mu) / _b)));
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double getInverseCDF(double? p)
	  {
		ArgChecker.notNull(p, "p");
		ArgChecker.isTrue(p >= 0 && p <= 1, "Probability must lie between 0 and 1 (inclusive)");
		return _mu - _b * Math.Sign(p - 0.5) * Math.Log(1 - 2 * Math.Abs(p - 0.5));
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double getPDF(double? x)
	  {
		ArgChecker.notNull(x, "x");
		return Math.Exp(-Math.Abs(x - _mu) / _b) / (2 * _b);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double nextRandom()
	  {
		double u = _engine.NextDouble() - 0.5;
		return _mu - _b * Math.Sign(u) * Math.Log(1 - 2 * Math.Abs(u));
	  }

	  /// <summary>
	  /// Gets the location parameter.
	  /// </summary>
	  /// <returns> The location parameter </returns>
	  public virtual double Mu
	  {
		  get
		  {
			return _mu;
		  }
	  }

	  /// <summary>
	  /// Gets the scale parameter.
	  /// </summary>
	  /// <returns> The scale parameter </returns>
	  public virtual double B
	  {
		  get
		  {
			return _b;
		  }
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		long temp;
		temp = System.BitConverter.DoubleToInt64Bits(_b);
		result = prime * result + (int)(temp ^ ((long)((ulong)temp >> 32)));
		temp = System.BitConverter.DoubleToInt64Bits(_mu);
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
		LaplaceDistribution other = (LaplaceDistribution) obj;
		if (System.BitConverter.DoubleToInt64Bits(_b) != Double.doubleToLongBits(other._b))
		{
		  return false;
		}
		return System.BitConverter.DoubleToInt64Bits(_mu) == Double.doubleToLongBits(other._mu);
	  }

	}

}