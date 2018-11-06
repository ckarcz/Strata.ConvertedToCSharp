using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Limit transform.
	/// <para>
	/// If a model parameter $x$ is constrained to be between two values $a \geq x
	/// \geq b$, the function to transform it to an unconstrained variable is $y$ is
	/// given by
	/// $$
	/// \begin{align*}
	/// y &= \tanh^{-1}\left(\frac{x - m}{s}\right)\\
	/// m &= \frac{a + b}{2}\\
	/// s &= \frac{b - a}{2}
	/// \end{align*}
	/// $$
	/// with the inverse transform
	/// $$
	/// \begin{align*}
	/// x &= s\tanh(y) + m\\
	/// \end{align*}
	/// $$
	/// </para>
	/// </summary>
	public class DoubleRangeLimitTransform : ParameterLimitsTransform
	{

	  private const double TANH_MAX = 25.0;
	  private readonly double _lower;
	  private readonly double _upper;
	  private readonly double _scale;
	  private readonly double _mid;

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="lower"> Lower limit </param>
	  /// <param name="upper"> Upper limit </param>
	  /// <exception cref="IllegalArgumentException"> If the upper limit is not greater than the lower limit </exception>
	  public DoubleRangeLimitTransform(double lower, double upper)
	  {
		ArgChecker.isTrue(upper > lower, "upper limit must be greater than lower");
		_lower = lower;
		_upper = upper;
		_mid = (lower + upper) / 2;
		_scale = (upper - lower) / 2;
	  }

	  /// <summary>
	  /// If $y > 25$, this returns $b$. If $y < -25$ returns $a$.
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double inverseTransform(double y)
	  {
		if (y > TANH_MAX)
		{
		  return _upper;
		}
		else if (y < -TANH_MAX)
		{
		  return _lower;
		}
		return _mid + _scale * TrigonometricFunctionUtils.tanh(y);
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="IllegalArgumentException"> If $x > b$ or $x < a$ </exception>
	  public virtual double transform(double x)
	  {
		ArgChecker.isTrue(x <= _upper && x >= _lower, "parameter out of range");
		if (x == _upper)
		{
		  return TANH_MAX;
		}
		else if (x == _lower)
		{
		  return -TANH_MAX;
		}
		return TrigonometricFunctionUtils.atanh((x - _mid) / _scale);
	  }

	  /// <summary>
	  /// If $|y| > 25$, this returns 0.
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double inverseTransformGradient(double y)
	  {
		if (y > TANH_MAX || y < -TANH_MAX)
		{
		  return 0.0;
		}
		double p = 2 * y;
		double ep = Math.Exp(p);
		double epp1 = ep + 1;
		return _scale * 4 * ep / (epp1 * epp1);
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="IllegalArgumentException"> If $x > b$ or $x < a$ </exception>
	  public virtual double transformGradient(double x)
	  {
		ArgChecker.isTrue(x <= _upper && x >= _lower, "parameter out of range");
		double t = (x - _mid) / _scale;
		return 1 / (_scale * (1 - t * t));
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		long temp;
		temp = System.BitConverter.DoubleToInt64Bits(_lower);
		result = prime * result + (int)(temp ^ ((long)((ulong)temp >> 32)));
		temp = System.BitConverter.DoubleToInt64Bits(_upper);
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
		DoubleRangeLimitTransform other = (DoubleRangeLimitTransform) obj;
		if (System.BitConverter.DoubleToInt64Bits(_lower) != Double.doubleToLongBits(other._lower))
		{
		  return false;
		}
		return System.BitConverter.DoubleToInt64Bits(_upper) == Double.doubleToLongBits(other._upper);
	  }

	}

}