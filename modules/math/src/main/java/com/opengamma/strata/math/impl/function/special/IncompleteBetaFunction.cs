/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function.special
{

	using MaxCountExceededException = org.apache.commons.math3.exception.MaxCountExceededException;
	using Beta = org.apache.commons.math3.special.Beta;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// The incomplete beta function is defined as:
	/// $$
	/// \begin{equation*}
	/// I_x(a, b)=\frac{B_x(a, b)}{B(a, b)}\int_0^x t^{a-1}(1-t)^{b-1}dt
	/// \end{equation*}
	/// $$
	/// where $a,b>0$.
	/// <para>
	/// This class uses the <a href="http://commons.apache.org/math/api-2.1/org/apache/commons/math/special/Beta.html">Commons Math library implementation</a> of the Beta function.
	/// </para>
	/// </summary>
	public class IncompleteBetaFunction : System.Func<double, double>
	{

	  private readonly double _a;
	  private readonly double _b;
	  private readonly double _eps;
	  private readonly int _maxIter;

	  /// <summary>
	  /// Creates an instance using the default values for the accuracy
	  /// ({@code 10^-12}) and number of iterations ({@code 10000}).
	  /// </summary>
	  /// <param name="a">  a, $a > 0$ </param>
	  /// <param name="b">  b, $b > 0$ </param>
	  public IncompleteBetaFunction(double a, double b) : this(a, b, 1e-12, 10000)
	  {
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="a">  a, $a > 0$ </param>
	  /// <param name="b">  b, $b > 0$ </param>
	  /// <param name="eps">  approximation accuracy, $\epsilon \geq 0$ </param>
	  /// <param name="maxIter">  maximum number of iterations, $\iter \geq 1$ </param>
	  public IncompleteBetaFunction(double a, double b, double eps, int maxIter)
	  {
		ArgChecker.isTrue(a > 0, "a must be > 0");
		ArgChecker.isTrue(b > 0, "b must be > 0");
		ArgChecker.isTrue(eps >= 0, "eps must not be negative");
		ArgChecker.isTrue(maxIter >= 1, "maximum number of iterations must be greater than zero");
		_a = a;
		_b = b;
		_eps = eps;
		_maxIter = maxIter;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Evaluates the function.
	  /// </summary>
	  /// <param name="x">  x </param>
	  /// <returns> the value of the function </returns>
	  /// <exception cref="IllegalArgumentException"> if $x < 0$ or $x > 1$ </exception>
	  public override double? apply(double? x)
	  {
		ArgChecker.isTrue(x >= 0 && x <= 1, "x must be in the range 0 to 1");
		try
		{
		  return Beta.regularizedBeta(x, _a, _b, _eps, _maxIter);
		}
		catch (MaxCountExceededException e)
		{
		  throw new MathException(e);
		}
	  }

	}

}