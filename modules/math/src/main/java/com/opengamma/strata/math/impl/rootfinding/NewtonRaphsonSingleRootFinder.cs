using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleFunction1D = com.opengamma.strata.math.impl.function.DoubleFunction1D;

	/// <summary>
	/// Class for finding the real root of a function within a range of $x$-values using the one-dimensional version of Newton's method.
	/// <para>
	/// For a function $f(x)$, the Taylor series expansion is given by:
	/// $$
	/// \begin{align*}
	/// f(x + \delta) \approx f(x) + f'(x)\delta + \frac{f''(x)}{2}\delta^2 + \cdots
	/// \end{align*}
	/// $$
	/// As delta approaches zero (and if the function is well-behaved), this gives 
	/// $$
	/// \begin{align*}
	/// \delta = -\frac{f(x)}{f'(x)}
	/// \end{align*}
	/// $$
	/// when $f(x + \delta) = 0$.
	/// </para>
	/// <para>
	/// There are several well-known problems with Newton's method, in particular when the range of values given includes a local
	/// maximum or minimum. In this situation, the next iterative step can shoot off to $\pm\infty$. This implementation
	/// currently does not attempt to correct for this: if the value of $x$ goes beyond the initial range of values $x_{low}$
	/// and $x_{high}$, an exception is thrown.
	/// </para>
	/// <para>
	/// If the function that is provided does not override the <seealso cref="com.opengamma.strata.math.impl.function.DoubleFunction1D#derivative()"/> method, then 
	/// the derivative is approximated using finite difference. This is undesirable for several reasons: (i) the extra function evaluations will lead
	/// to slower convergence; and (ii) the choice of shift size is very important (too small and the result will be dominated by rounding errors, too large
	/// and convergence will be even slower). Use of another root-finder is recommended in this case.
	/// </para>
	/// </summary>
	// CSOFF: JavadocMethod
	public class NewtonRaphsonSingleRootFinder : RealSingleRootFinder
	{

	  private const int MAX_ITER = 10000;
	  private readonly double _accuracy;

	  /// <summary>
	  /// Default constructor. Sets accuracy to 1e-12.
	  /// </summary>
	  public NewtonRaphsonSingleRootFinder() : this(1e-12)
	  {
	  }

	  /// <summary>
	  /// Takes the accuracy of the root as a parameter - this is the maximum difference
	  /// between the true root and the returned value that is allowed. 
	  /// If this is negative, then the absolute value is used.
	  /// </summary>
	  /// <param name="accuracy"> The accuracy </param>
	  public NewtonRaphsonSingleRootFinder(double accuracy)
	  {
		_accuracy = Math.Abs(accuracy);
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="MathException"> If the root is not found in 1000 attempts; if the Newton
	  ///   step takes the estimate for the root outside the original bounds. </exception>
	  public override double? getRoot(System.Func<double, double> function, double? x1, double? x2)
	  {
		ArgChecker.notNull(function, "function");
		return getRoot(DoubleFunction1D.from(function), x1, x2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual double? getRoot(System.Func<double, double> function, double? x)
	  {
		ArgChecker.notNull(function, "function");
		ArgChecker.notNull(x, "x");
		DoubleFunction1D f = DoubleFunction1D.from(function);
		return getRoot(f, f.derivative(), x);
	  }

	  /// <summary>
	  /// Uses the <seealso cref="DoubleFunction1D#derivative()"/> method. <i>x<sub>1</sub></i> and
	  /// <i>x<sub>2</sub></i> do not have to be increasing.
	  /// </summary>
	  /// <param name="function"> The function, not null </param>
	  /// <param name="x1"> The first bound of the root, not null </param>
	  /// <param name="x2"> The second bound of the root, not null </param>
	  /// <returns> The root </returns>
	  /// <exception cref="MathException"> If the root is not found in 1000 attempts; if the Newton
	  ///   step takes the estimate for the root outside the original bounds. </exception>
	  public virtual double? getRoot(DoubleFunction1D function, double? x1, double? x2)
	  {
		ArgChecker.notNull(function, "function");
		return getRoot(function, function.derivative(), x1, x2);
	  }

	  /// <summary>
	  /// Uses the <seealso cref="DoubleFunction1D#derivative()"/> method. This method uses an initial
	  /// guess for the root, rather than bounds.
	  /// </summary>
	  /// <param name="function"> The function, not null </param>
	  /// <param name="x"> The initial guess for the root, not null </param>
	  /// <returns> The root </returns>
	  /// <exception cref="MathException"> If the root is not found in 1000 attempts. </exception>
	  public virtual double? getRoot(DoubleFunction1D function, double? x)
	  {
		ArgChecker.notNull(function, "function");
		return getRoot(function, function.derivative(), x);
	  }

	  /// <summary>
	  /// Uses the function and its derivative. </summary>
	  /// <param name="function"> The function, not null </param>
	  /// <param name="derivative"> The derivative, not null </param>
	  /// <param name="x1"> The first bound of the root, not null </param>
	  /// <param name="x2"> The second bound of the root, not null </param>
	  /// <returns> The root </returns>
	  /// <exception cref="MathException"> If the root is not found in 1000 attempts; if the Newton
	  ///   step takes the estimate for the root outside the original bounds. </exception>
	  public virtual double? getRoot(System.Func<double, double> function, System.Func<double, double> derivative, double? x1, double? x2)
	  {
		checkInputs(function, x1, x2);
		ArgChecker.notNull(derivative, "derivative");
		return getRoot(DoubleFunction1D.from(function), DoubleFunction1D.from(derivative), x1, x2);
	  }

	  /// <summary>
	  /// Uses the function and its derivative. This method uses an initial guess for the root, rather than bounds. </summary>
	  /// <param name="function"> The function, not null </param>
	  /// <param name="derivative"> The derivative, not null </param>
	  /// <param name="x"> The initial guess for the root, not null </param>
	  /// <returns> The root </returns>
	  /// <exception cref="MathException"> If the root is not found in 1000 attempts. </exception>
	  public virtual double? getRoot(System.Func<double, double> function, System.Func<double, double> derivative, double? x)
	  {
		return getRoot(DoubleFunction1D.from(function), DoubleFunction1D.from(derivative), x);
	  }

	  /// <summary>
	  /// Uses the function and its derivative. </summary>
	  /// <param name="function"> The function, not null </param>
	  /// <param name="derivative"> The derivative, not null </param>
	  /// <param name="x1"> The first bound of the root, not null </param>
	  /// <param name="x2"> The second bound of the root, not null </param>
	  /// <returns> The root </returns>
	  /// <exception cref="MathException"> If the root is not found in 1000 attempts; if the Newton
	  ///   step takes the estimate for the root outside the original bounds. </exception>
	  public virtual double? getRoot(DoubleFunction1D function, DoubleFunction1D derivative, double? x1, double? x2)
	  {
		checkInputs(function, x1, x2);
		ArgChecker.notNull(derivative, "derivative function");
		double y1 = function.applyAsDouble(x1);
		if (Math.Abs(y1) < _accuracy)
		{
		  return x1;
		}
		double y2 = function.applyAsDouble(x2);
		if (Math.Abs(y2) < _accuracy)
		{
		  return x2;
		}
		double x = (x1 + x2) / 2;
		double x3 = y2 < 0 ? x2.Value : x1.Value;
		double x4 = y2 < 0 ? x1.Value : x2.Value;
		double xLower = x1 > x2 ? x2 : x1;
		double xUpper = x1 > x2 ? x1 : x2;
		for (int i = 0; i < MAX_ITER; i++)
		{
		  double y = function.applyAsDouble(x);
		  double dy = derivative.applyAsDouble(x);
		  double dx = -y / dy;
		  if (Math.Abs(dx) <= _accuracy)
		  {
			return x + dx;
		  }
		  x += dx;
		  if (x < xLower || x > xUpper)
		  {
			dx = (x4 - x3) / 2;
			x = x3 + dx;
		  }
		  if (y < 0)
		  {
			x3 = x;
		  }
		  else
		  {
			x4 = x;
		  }
		}
		throw new MathException("Could not find root in " + MAX_ITER + " attempts");
	  }

	  /// <summary>
	  /// Uses the function and its derivative. This method uses an initial guess for the root, rather than bounds. </summary>
	  /// <param name="function"> The function, not null </param>
	  /// <param name="derivative"> The derivative, not null </param>
	  /// <param name="x"> The initial guess for the root, not null </param>
	  /// <returns> The root </returns>
	  /// <exception cref="MathException"> If the root is not found in 1000 attempts. </exception>
	  public virtual double? getRoot(DoubleFunction1D function, DoubleFunction1D derivative, double? x)
	  {
		ArgChecker.notNull(function, "function");
		ArgChecker.notNull(derivative, "derivative function");
		ArgChecker.notNull(x, "x");
		double root = x.Value;
		for (int i = 0; i < MAX_ITER; i++)
		{
		  double y = function.applyAsDouble(root);
		  double dy = derivative.applyAsDouble(root);
		  double dx = y / dy;
		  if (Math.Abs(dx) <= _accuracy)
		  {
			return root - dx;
		  }
		  root -= dx;
		}
		throw new MathException("Could not find root in " + MAX_ITER + " attempts");
	  }

	}

}