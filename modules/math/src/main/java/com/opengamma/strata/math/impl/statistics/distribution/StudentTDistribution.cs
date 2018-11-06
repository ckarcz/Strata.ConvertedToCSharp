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
	using StudentT = com.opengamma.strata.math.impl.cern.StudentT;
	using GammaFunction = com.opengamma.strata.math.impl.function.special.GammaFunction;
	using InverseIncompleteBetaFunction = com.opengamma.strata.math.impl.function.special.InverseIncompleteBetaFunction;

	/// <summary>
	/// Student's T-distribution is a continuous probability distribution with probability density function
	/// $$
	/// \begin{align*}
	/// f(x) = \frac{\Gamma\left(\frac{\nu + 1}{2}\right)}{\sqrt{\nu\pi}\Gamma(\left(\frac{\nu}{2}\right)}\left(1 + \frac{x^2}{\nu}\right)^{-\frac{1}{2}(\nu + 1)}
	/// \end{align*}
	/// $$
	/// where $\nu$ is the number of degrees of freedom and $\Gamma$ is the Gamma function (<seealso cref="GammaFunction"/>).
	/// </summary>
	public class StudentTDistribution : ProbabilityDistribution<double>
	{
	  // TODO need a better seed
	  private readonly double _degFreedom;
	  private readonly StudentT _dist;
	  private readonly System.Func<double, double> _beta;

	  /// <param name="degFreedom"> The number of degrees of freedom, not negative or zero </param>
	  public StudentTDistribution(double degFreedom) : this(degFreedom, new MersenneTwister64(DateTime.Now))
	  {
	  }

	  /// <param name="degFreedom"> The number of degrees of freedom, not negative or zero </param>
	  /// <param name="engine"> A generator of uniform random numbers, not null </param>
	  public StudentTDistribution(double degFreedom, RandomEngine engine)
	  {
		ArgChecker.isTrue(degFreedom > 0, "degrees of freedom");
		ArgChecker.notNull(engine, "engine");
		_degFreedom = degFreedom;
		_dist = new StudentT(degFreedom, engine);
		_beta = new InverseIncompleteBetaFunction(degFreedom / 2.0, 0.5);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double getCDF(double? x)
	  {
		ArgChecker.notNull(x, "x");
		return _dist.cdf(x.Value);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double getPDF(double? x)
	  {
		ArgChecker.notNull(x, "x");
		return _dist.pdf(x.Value);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double nextRandom()
	  {
		return _dist.NextDouble();
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// The inverse cdf is given by:
	  /// $$
	  /// \begin{align*}
	  /// F(P) &= \mathrm{sign}(p - \frac{1}{2})\sqrt{\frac{\nu}{x - 1}}\\
	  /// x &= B(2 \min(p, 1-p)) 
	  /// \end{align*}
	  /// $$
	  /// where $B$ is the inverse incomplete Beta function (<seealso cref="InverseIncompleteBetaFunction"/>).
	  /// </summary>
	  public virtual double getInverseCDF(double? p)
	  {
		ArgChecker.notNull(p, "p");
		ArgChecker.isTrue(p >= 0 && p <= 1, "Probability must be >= 0 and <= 1");
		double x = _beta.apply(2 * Math.Min(p, 1 - p));
		return Math.Sign(p - 0.5) * Math.Sqrt(_degFreedom * (1.0 / x - 1));
	  }

	  /// <returns> The number of degrees of freedom </returns>
	  public virtual double DegreesOfFreedom
	  {
		  get
		  {
			return _degFreedom;
		  }
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		long temp;
		temp = System.BitConverter.DoubleToInt64Bits(_degFreedom);
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
		StudentTDistribution other = (StudentTDistribution) obj;
		return System.BitConverter.DoubleToInt64Bits(_degFreedom) == Double.doubleToLongBits(other._degFreedom);
	  }

	}

}