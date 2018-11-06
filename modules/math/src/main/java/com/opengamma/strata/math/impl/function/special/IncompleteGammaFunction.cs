/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function.special
{

	using MaxCountExceededException = org.apache.commons.math3.exception.MaxCountExceededException;
	using Gamma = org.apache.commons.math3.special.Gamma;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// The incomplete gamma function is defined as:
	/// $$
	/// \begin{equation*}
	/// P(a, x) = \frac{\gamma(a, x)}{\Gamma(a)}\int_0^x e^{-t}t^{a-1}dt
	/// \end{equation*}
	/// $$
	/// where $a > 0$.
	/// <para>
	/// This class is a wrapper for the Commons Math library implementation of the incomplete gamma
	/// function <a href="http://commons.apache.org/math/api-2.1/index.html">link</a>
	/// </para>
	/// </summary>
	public class IncompleteGammaFunction : System.Func<double, double>
	{

	  private readonly int _maxIter;
	  private readonly double _eps;
	  private readonly double _a;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="a">  the value </param>
	  public IncompleteGammaFunction(double a)
	  {
		ArgChecker.notNegativeOrZero(a, "a");
		_maxIter = 100000;
		_eps = 1e-12;
		_a = a;
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="a">  the value </param>
	  /// <param name="maxIter">  the maximum iterations </param>
	  /// <param name="eps">  the epsilon </param>
	  public IncompleteGammaFunction(double a, int maxIter, double eps)
	  {
		ArgChecker.notNegativeOrZero(a, "a");
		ArgChecker.notNegative(eps, "eps");
		if (maxIter < 1)
		{
		  throw new System.ArgumentException("Must have at least one iteration");
		}
		_maxIter = maxIter;
		_eps = eps;
		_a = a;
	  }

	  //-------------------------------------------------------------------------
	  public override double? apply(double? x)
	  {
		try
		{
		  return Gamma.regularizedGammaP(_a, x, _eps, _maxIter);
		}
		catch (MaxCountExceededException e)
		{
		  throw new MathException(e);
		}
	  }

	}

}