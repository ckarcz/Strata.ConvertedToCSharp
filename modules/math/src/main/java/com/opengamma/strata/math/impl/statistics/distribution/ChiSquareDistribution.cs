using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.distribution
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ChiSquare = com.opengamma.strata.math.impl.cern.ChiSquare;
	using MersenneTwister64 = com.opengamma.strata.math.impl.cern.MersenneTwister64;
	using RandomEngine = com.opengamma.strata.math.impl.cern.RandomEngine;
	using InverseIncompleteGammaFunction = com.opengamma.strata.math.impl.function.special.InverseIncompleteGammaFunction;

	/// <summary>
	/// A $\chi^2$ distribution with $k$ degrees of freedom is the distribution of
	/// the sum of squares of $k$ independent standard normal random variables with
	/// cdf and inverse cdf
	/// $$
	/// \begin{align*}
	/// F(x) &=\frac{\gamma\left(\frac{k}{2}, \frac{x}{2}\right)}{\Gamma\left(\frac{k}{2}\right)}\\
	/// F^{-1}(p) &= 2\gamma^{-1}\left(\frac{k}{2}, p\right)
	/// \end{align*}
	/// $$
	/// where $\gamma(y, z)$ is the lower incomplete Gamma function and $\Gamma(y)$
	/// is the Gamma function.  The pdf is given by:
	/// $$
	/// \begin{align*}
	/// f(x)=\frac{x^{\frac{k}{2}-1}e^{-\frac{x}{2}}}{2^{\frac{k}{2}}\Gamma\left(\frac{k}{2}\right)}
	/// \end{align*}
	/// $$
	/// 
	/// </summary>
	public class ChiSquareDistribution : ProbabilityDistribution<double>
	{

	  private readonly System.Func<double, double, double> _inverseFunction = new InverseIncompleteGammaFunction();
	  private readonly ChiSquare _chiSquare;
	  private readonly double _degrees;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="degrees"> The degrees of freedom of the distribution, not less than one </param>
	  public ChiSquareDistribution(double degrees) : this(degrees, new MersenneTwister64(DateTime.Now))
	  {
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="degrees"> The degrees of freedom of the distribution, not less than one </param>
	  /// <param name="engine"> A uniform random number generator, not null </param>
	  public ChiSquareDistribution(double degrees, RandomEngine engine)
	  {
		ArgChecker.isTrue(degrees >= 1, "Degrees of freedom must be greater than or equal to one");
		ArgChecker.notNull(engine, "engine");
		_chiSquare = new ChiSquare(degrees, engine);
		_degrees = degrees;
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double getCDF(double? x)
	  {
		ArgChecker.notNull(x, "x");
		return _chiSquare.cdf(x.Value);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double getPDF(double? x)
	  {
		ArgChecker.notNull(x, "x");
		return _chiSquare.pdf(x.Value);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double getInverseCDF(double? p)
	  {
		ArgChecker.notNull(p, "p");
		ArgChecker.isTrue(p >= 0 && p <= 1, "Probability must lie between 0 and 1");
		return 2 * _inverseFunction.applyAsDouble(0.5 * _degrees, p);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double nextRandom()
	  {
		return _chiSquare.NextDouble();
	  }

	  /// <summary>
	  /// Gets the degrees of freedom.
	  /// </summary>
	  /// <returns> The number of degrees of freedom </returns>
	  public virtual double DegreesOfFreedom
	  {
		  get
		  {
			return _degrees;
		  }
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		long temp;
		temp = System.BitConverter.DoubleToInt64Bits(_degrees);
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
		ChiSquareDistribution other = (ChiSquareDistribution) obj;
		return System.BitConverter.DoubleToInt64Bits(_degrees) == Double.doubleToLongBits(other._degrees);
	  }

	}

}