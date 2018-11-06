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
	/// If a model parameter $x$ is constrained to be either above or below some
	/// level $a$ (i.e. $x > a$ or $x < a$), the function to transform it to an
	/// unconstrained variable $y$ is given by
	/// $$
	/// \begin{align*}
	/// y = 
	/// \begin{cases}
	/// \ln(e^{x-a} - 1)\quad & x > a\\
	/// a - \ln(e^{a-x} - 1)\quad & x < a
	/// \end{cases}
	/// \end{align*}
	/// $$
	/// with inverse transform
	/// $$
	/// \begin{align*}
	/// x = 
	/// \begin{cases}
	/// a + \ln(e^y + 1)\quad & x > a\\
	/// a - \ln(e^y + 1)\quad & x < a
	/// \end{cases}
	/// \end{align*}
	/// $$
	/// For large $y > 50$, this becomes
	/// $$
	/// \begin{align*}
	/// y = 
	/// \begin{cases}
	/// x - a\quad & x > a\\
	/// a - x\quad & x < a
	/// \end{cases}
	/// \end{align*}
	/// $$
	/// with inverse transform
	/// $$
	/// \begin{align*}
	/// x = 
	/// \begin{cases}
	/// a + y\quad & x > a\\
	/// a - y\quad & x < a
	/// \end{cases}
	/// \end{align*}
	/// $$
	/// so any value of $y$ will give a value of $x$.
	/// </summary>
	public class SingleRangeLimitTransform : ParameterLimitsTransform
	{

	  private const double EXP_MAX = 50.0;
	  private readonly double _limit;
	  private readonly int _sign;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="a"> The limit level </param>
	  /// <param name="limitType"> Type of the limit for the parameter </param>
	  public SingleRangeLimitTransform(double a, ParameterLimitsTransform_LimitType limitType)
	  {
		_limit = a;
		_sign = limitType == ParameterLimitsTransform_LimitType.GREATER_THAN ? 1 : -1;
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double inverseTransform(double y)
	  {
		if (y > EXP_MAX)
		{
		  return _limit + _sign * y;
		}
		else if (y < -EXP_MAX)
		{
		  return _limit;
		}
		return _limit + _sign * Math.Log(Math.Exp(y) + 1);
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="IllegalArgumentException"> If the value of $x$ is not consistent with the limit
	  ///   (e.g. the limit is $x > a$ and $x$ is less than $a$ </exception>
	  public virtual double transform(double x)
	  {
		ArgChecker.isTrue(_sign * x >= _sign * _limit, "x not in limit");
		if (x == _limit)
		{
		  return -EXP_MAX;
		}
		double r = _sign * (x - _limit);
		if (r > EXP_MAX)
		{
		  return r;
		}
		return Math.Log(Math.Exp(r) - 1);
	  }

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double inverseTransformGradient(double y)
	  {
		if (y > EXP_MAX)
		{
		  return _sign;
		}
		double temp = Math.Exp(y);
		return _sign * temp / (temp + 1);
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="IllegalArgumentException"> If the value of $x$ is not consistent with the limit
	  ///   (e.g. the limit is $x > a$ and $x$ is less than $a$ </exception>
	  public virtual double transformGradient(double x)
	  {
		ArgChecker.isTrue(_sign * x >= _sign * _limit, "x not in limit");
		double r = _sign * (x - _limit);
		if (r > EXP_MAX)
		{
		  return 1.0;
		}
		double temp = Math.Exp(r);
		return _sign * temp / (temp - 1);
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		long temp;
		temp = System.BitConverter.DoubleToInt64Bits(_limit);
		result = prime * result + (int)(temp ^ ((long)((ulong)temp >> 32)));
		result = prime * result + _sign;
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
		SingleRangeLimitTransform other = (SingleRangeLimitTransform) obj;
		if (System.BitConverter.DoubleToInt64Bits(_limit) != Double.doubleToLongBits(other._limit))
		{
		  return false;
		}
		return _sign == other._sign;
	  }

	}

}