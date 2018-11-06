using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.distribution
{

	using DoubleMath = com.google.common.math.DoubleMath;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MersenneTwister64 = com.opengamma.strata.math.impl.cern.MersenneTwister64;
	using RandomEngine = com.opengamma.strata.math.impl.cern.RandomEngine;

	/// <summary>
	/// Calculates the Pareto distribution.
	/// <para>
	/// The generalized Pareto distribution is a family of power law probability
	/// distributions with location parameter $\mu$, shape parameter $\xi$ and scale
	/// parameter $\sigma$, where
	/// $$
	/// \begin{eqnarray*}
	/// \mu&\in&\Re,\\
	/// \xi&\in&\Re,\\
	/// \sigma&>&0
	/// \end{eqnarray*}
	/// $$
	/// and with support
	/// $$
	/// \begin{eqnarray*}
	/// x\geq\mu\quad\quad\quad(\xi\geq 0)\\
	/// \mu\leq x\leq\mu-\frac{\sigma}{\xi}\quad(\xi<0)
	/// \end{eqnarray*}
	/// $$
	/// The cdf is given by:
	/// $$
	/// \begin{align*}
	/// F(z)&=1-\left(1 + \xi z\right)^{-\frac{1}{\xi}}\\
	/// z&=\frac{x-\mu}{\sigma}
	/// \end{align*}
	/// $$
	/// and the pdf is given by:
	/// $$
	/// \begin{align*}
	/// f(z)&=\frac{\left(1+\xi z\right)^{-\left(\frac{1}{\xi} + 1\right)}}{\sigma}\\
	/// z&=\frac{x-\mu}{\sigma}
	/// \end{align*}
	/// $$
	/// Given a uniform random number variable $U$ drawn from the interval $(0,1]$, a
	/// Pareto-distributed random variable with parameters $\mu$, $\sigma$ and
	/// $\xi$ is given by
	/// $$
	/// \begin{align*}
	/// X=\mu + \frac{\sigma\left(U^{-\xi}-1\right)}{\xi}\sim GPD(\mu,\sigma,\xi)
	/// \end{align*}
	/// $$
	/// </para>
	/// </summary>
	public class GeneralizedParetoDistribution : ProbabilityDistribution<double>
	{
	  // TODO check cdf, pdf for support
	  private readonly double _mu;
	  private readonly double _sigma;
	  private readonly double _ksi;
	  // TODO better seed
	  private readonly RandomEngine _engine;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="mu"> The location parameter </param>
	  /// <param name="sigma"> The scale parameter, not negative or zero </param>
	  /// <param name="ksi"> The shape parameter, not zero </param>
	  public GeneralizedParetoDistribution(double mu, double sigma, double ksi) : this(mu, sigma, ksi, new MersenneTwister64(DateTime.Now))
	  {
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="mu"> The location parameter </param>
	  /// <param name="sigma"> The scale parameter </param>
	  /// <param name="ksi"> The shape parameter </param>
	  /// <param name="engine"> A uniform random number generator, not null </param>
	  public GeneralizedParetoDistribution(double mu, double sigma, double ksi, RandomEngine engine)
	  {
		ArgChecker.isTrue(sigma > 0, "sigma must be > 0");
		ArgChecker.isTrue(!DoubleMath.fuzzyEquals(ksi, 0d, 1e-15), "ksi cannot be zero");
		ArgChecker.notNull(engine, "engine");
		_mu = mu;
		_sigma = sigma;
		_ksi = ksi;
		_engine = engine;
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
	  public virtual double Sigma
	  {
		  get
		  {
			return _sigma;
		  }
	  }

	  /// <summary>
	  /// Gets the shape parameter.
	  /// </summary>
	  /// <returns> The shape parameter </returns>
	  public virtual double Ksi
	  {
		  get
		  {
			return _ksi;
		  }
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="IllegalArgumentException"> If $x \not\in$ support </exception>
	  public virtual double getCDF(double? x)
	  {
		ArgChecker.notNull(x, "x");
		return 1 - Math.Pow(1 + _ksi * getZ(x.Value), -1.0 / _ksi);
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
	  /// {@inheritDoc} </summary>
	  /// <exception cref="IllegalArgumentException"> If $x \not\in$ support </exception>
	  public virtual double getPDF(double? x)
	  {
		ArgChecker.notNull(x, "x");
		return Math.Pow(1 + _ksi * getZ(x.Value), -(1.0 / _ksi + 1)) / _sigma;
	  }

	  /// <summary>
	  /// {@inheritDoc} 
	  /// </summary>
	  public virtual double nextRandom()
	  {
		return _mu + _sigma * (Math.Pow(_engine.NextDouble(), -_ksi) - 1) / _ksi;
	  }

	  private double getZ(double x)
	  {
		if (_ksi > 0 && x < _mu)
		{
		  throw new System.ArgumentException("Support for GPD is in the range x >= mu if ksi > 0");
		}
		if (_ksi < 0 && (x <= _mu || x >= _mu - _sigma / _ksi))
		{
		  throw new System.ArgumentException("Support for GPD is in the range mu <= x <= mu - sigma / ksi if ksi < 0");
		}
		return (x - _mu) / _sigma;
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		long temp;
		temp = System.BitConverter.DoubleToInt64Bits(_ksi);
		result = prime * result + (int)(temp ^ ((long)((ulong)temp >> 32)));
		temp = System.BitConverter.DoubleToInt64Bits(_mu);
		result = prime * result + (int)(temp ^ ((long)((ulong)temp >> 32)));
		temp = System.BitConverter.DoubleToInt64Bits(_sigma);
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
		GeneralizedParetoDistribution other = (GeneralizedParetoDistribution) obj;
		if (System.BitConverter.DoubleToInt64Bits(_ksi) != Double.doubleToLongBits(other._ksi))
		{
		  return false;
		}
		if (System.BitConverter.DoubleToInt64Bits(_mu) != Double.doubleToLongBits(other._mu))
		{
		  return false;
		}
		return System.BitConverter.DoubleToInt64Bits(_sigma) == Double.doubleToLongBits(other._sigma);
	  }

	}

}