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

	/// 
	/// <summary>
	/// The generalized extreme value distribution is a family of continuous probability distributions that combines the Gumbel (type I),
	/// Fr&eacute;chet (type II) and Weibull (type III) families of distributions.
	/// <para>
	/// This distribution has location parameter $\mu$, shape parameter $\xi$
	/// and scale parameter $\sigma$, with
	/// $$
	/// \begin{align*}
	/// \mu&\in\Re,\\
	/// \xi&\in\Re,\\
	/// \sigma&>0
	/// \end{align*}
	/// $$
	/// and support
	/// $$
	/// \begin{align*}
	/// x\in
	/// \begin{cases}
	/// \left[\mu - \frac{\sigma}{\xi}, +\infty\right) & \text{when } \xi > 0\\
	/// (-\infty,+\infty) & \text{when } \xi = 0\\\\
	/// \left(-\infty, \mu - \frac{\sigma}{\xi}\right] & \text{when } \xi < 0
	/// \end{cases}
	/// \end{align*}
	/// $$
	/// The cdf is given by:
	/// $$
	/// \begin{align*}
	/// F(x) &=e^{-t(x)}\\
	/// t(x)&=
	/// \begin{cases}
	/// \left(1 + \xi\frac{x-\mu}{\sigma}\right)^{-\frac{1}{\xi}} & \text{if } \xi \neq 0,\\
	/// e^{-\frac{x-\mu}{\sigma}} & \text{if } \xi = 0.
	/// \end{cases}
	/// \end{align*}
	/// $$
	/// and the pdf by:
	/// $$
	/// \begin{align*}
	/// f(x)&=\frac{t(x)^{\xi + 1}e^{-t(x)}}{\sigma}\quad\\
	/// t(x)&=
	/// \begin{cases}
	/// \left(1 + \xi\frac{x-\mu}{\sigma}\right)^{-\frac{1}{\xi}} & \text{if } \xi \neq 0,\\
	/// e^{-\frac{x-\mu}{\sigma}} & \text{if } \xi = 0.
	/// \end{cases}
	/// \end{align*}
	/// $$
	/// 
	/// </para>
	/// </summary>
	public class GeneralizedExtremeValueDistribution : ProbabilityDistribution<double>
	{

	  private readonly double _mu;
	  private readonly double _sigma;
	  private readonly double _ksi;
	  private readonly bool _ksiIsZero;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="mu"> The location parameter </param>
	  /// <param name="sigma"> The scale parameter, not negative or zero </param>
	  /// <param name="ksi"> The shape parameter </param>
	  public GeneralizedExtremeValueDistribution(double mu, double sigma, double ksi)
	  {
		ArgChecker.isTrue(sigma >= 0, "sigma must be >= 0");
		_mu = mu;
		_sigma = sigma;
		_ksi = ksi;
		_ksiIsZero = DoubleMath.fuzzyEquals(ksi, 0d, 1e-13);
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="IllegalArgumentException"> If $x \not\in$ support </exception>
	  public virtual double getCDF(double? x)
	  {
		ArgChecker.notNull(x, "x");
		return Math.Exp(-getT(x.Value));
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
		double t = getT(x.Value);
		return Math.Pow(t, _ksi + 1) * Math.Exp(-t) / _sigma;
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <returns> Not supported </returns>
	  /// <exception cref="UnsupportedOperationException"> always </exception>
	  public virtual double nextRandom()
	  {
		throw new System.NotSupportedException();
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

	  private double getT(double x)
	  {
		if (_ksiIsZero)
		{
		  return Math.Exp(-(x - _mu) / _sigma);
		}
		if (_ksi < 0 && x > _mu - _sigma / _ksi)
		{
		  throw new System.ArgumentException("Support for GEV is in the range -infinity -> mu - sigma / ksi when ksi < 0");
		}
		if (_ksi > 0 && x < _mu - _sigma / _ksi)
		{
		  throw new System.ArgumentException("Support for GEV is in the range mu - sigma / ksi -> +infinity when ksi > 0");
		}
		return Math.Pow(1 + _ksi * (x - _mu) / _sigma, -1.0 / _ksi);
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
		GeneralizedExtremeValueDistribution other = (GeneralizedExtremeValueDistribution) obj;
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