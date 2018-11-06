using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.distribution
{
	using MaxCountExceededException = org.apache.commons.math3.exception.MaxCountExceededException;
	using Gamma = org.apache.commons.math3.special.Gamma;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using GammaFunction = com.opengamma.strata.math.impl.function.special.GammaFunction;

	/// <summary>
	/// The non-central chi-squared distribution is a continuous probability
	/// distribution with probability density function
	/// $$
	/// \begin{align*}
	/// f_r(x) = \frac{e^-\frac{x + \lambda}{2}x^{\frac{r}{2} - 1}}{2^{\frac{r}{2}}}\sum_{k=0}^\infty \frac{(\lambda k)^k}{2^{2k}k!\Gamma(k + \frac{r}{2})}
	/// \end{align*}
	/// $$
	/// where $r$ is the number of degrees of freedom, $\lambda$ is the
	/// non-centrality parameter and $\Gamma$ is the Gamma function ({@link
	/// GammaFunction}).
	/// <para>
	/// For the case where $r + \lambda > 2000$, the implementation of the cdf is taken from "An Approximation for the Noncentral Chi-Squared Distribution", Fraser et al.
	/// (<a href="http://fisher.utstat.toronto.edu/dfraser/documents/192.pdf">link</a>). Otherwise, the algorithm is taken from "Computing the Non-Central Chi-Squared Distribution Function", Ding.
	/// </para>
	/// </summary>
	// CSOFF: AbbreviationAsWordInName
	public class NonCentralChiSquaredDistribution : ProbabilityDistribution<double>
	{

	  private readonly double _lambdaOverTwo;
	  private readonly int _k;
	  private readonly double _dofOverTwo;
	  private readonly double _pStart;
	  private readonly double _eps = 1e-16;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="degrees"> The number of degrees of freedom, not negative or zero </param>
	  /// <param name="nonCentrality"> The non-centrality parameter, not negative </param>
	  public NonCentralChiSquaredDistribution(double degrees, double nonCentrality)
	  {
		ArgChecker.isTrue(degrees > 0, "degrees of freedom must be > 0, have " + degrees);
		ArgChecker.isTrue(nonCentrality >= 0, "non-centrality must be >= 0, have " + nonCentrality);
		_dofOverTwo = degrees / 2.0;
		_lambdaOverTwo = nonCentrality / 2.0;
		_k = (int) (long)Math.Round(_lambdaOverTwo, MidpointRounding.AwayFromZero);

		if (_lambdaOverTwo == 0)
		{
		  _pStart = 0.0;
		}
		else
		{
		  double logP = -_lambdaOverTwo + _k * Math.Log(_lambdaOverTwo) - Gamma.logGamma(_k + 1);
		  _pStart = Math.Exp(logP);
		}
	  }

	  private double getFraserApproxCDF(double x)
	  {
		double s = Math.Sqrt(_lambdaOverTwo * 2.0);
		double mu = Math.Sqrt(x);
		double z;
		if (System.BitConverter.DoubleToInt64Bits(mu) == Double.doubleToLongBits(s))
		{
		  z = (1 - _dofOverTwo * 2.0) / 2 / s;
		}
		else
		{
		  z = mu - s - (_dofOverTwo * 2.0 - 1) / 2 * (Math.Log(mu) - Math.Log(s)) / (mu - s);
		}
		return (new NormalDistribution(0, 1)).getCDF(z);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double getCDF(double? x)
	  {
		ArgChecker.notNull(x, "x");
		if (x < 0)
		{
		  return 0.0;
		}

		if ((_dofOverTwo + _lambdaOverTwo) > 1000)
		{
		  return getFraserApproxCDF(x.Value);
		}

		double regGammaStart = 0;
		double halfX = x / 2.0;
		double logX = Math.Log(halfX);
		try
		{
		  regGammaStart = Gamma.regularizedGammaP(_dofOverTwo + _k, halfX);
		}
		catch (MaxCountExceededException ex)
		{
		  throw new MathException(ex);
		}

		double sum = _pStart * regGammaStart;
		double oldSum = double.NegativeInfinity;
		double p = _pStart;
		double regGamma = regGammaStart;
		double temp;
		int i = _k;

		// first add terms below _k
		while (i > 0 && Math.Abs(sum - oldSum) / sum > _eps)
		{
		  i--;
		  p *= (i + 1) / _lambdaOverTwo;
		  temp = (_dofOverTwo + i) * logX - halfX - Gamma.logGamma(_dofOverTwo + i + 1);
		  regGamma += Math.Exp(temp);
		  oldSum = sum;
		  sum += p * regGamma;
		}

		p = _pStart;
		regGamma = regGammaStart;
		oldSum = double.NegativeInfinity;
		i = _k;
		while (Math.Abs(sum - oldSum) / sum > _eps)
		{
		  i++;
		  p *= _lambdaOverTwo / i;
		  temp = (_dofOverTwo + i - 1) * logX - halfX - Gamma.logGamma(_dofOverTwo + i);
		  regGamma -= Math.Exp(temp);
		  oldSum = sum;
		  sum += p * regGamma;
		}

		return sum;
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
	  /// <returns> Not supported </returns>
	  /// <exception cref="UnsupportedOperationException"> always </exception>
	  public virtual double getPDF(double? x)
	  {
		throw new System.NotSupportedException();
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
	  /// Gets the number of degrees of freedom.
	  /// </summary>
	  /// <returns> The number of degrees of freedom </returns>
	  public virtual double Degrees
	  {
		  get
		  {
			return _dofOverTwo * 2.0;
		  }
	  }

	  /// <summary>
	  /// Gets the non-centrality parameter.
	  /// </summary>
	  /// <returns> The non-centrality parameter </returns>
	  public virtual double NonCentrality
	  {
		  get
		  {
			return _lambdaOverTwo * 2.0;
		  }
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		long temp;
		temp = System.BitConverter.DoubleToInt64Bits(_dofOverTwo);
		result = prime * result + (int)(temp ^ ((long)((ulong)temp >> 32)));
		temp = System.BitConverter.DoubleToInt64Bits(_lambdaOverTwo);
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
		NonCentralChiSquaredDistribution other = (NonCentralChiSquaredDistribution) obj;
		if (System.BitConverter.DoubleToInt64Bits(_dofOverTwo) != Double.doubleToLongBits(other._dofOverTwo))
		{
		  return false;
		}
		return System.BitConverter.DoubleToInt64Bits(_lambdaOverTwo) == Double.doubleToLongBits(other._lambdaOverTwo);
	  }

	}

}