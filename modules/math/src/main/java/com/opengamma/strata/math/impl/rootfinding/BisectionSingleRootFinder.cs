using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{

	/// <summary>
	/// Finds a single root of a function using the bisection method.
	/// <para>
	/// If a root of a function $f(x)$ is bounded by two values $x_1$ and $x_2$,
	/// then $f(x_1)f(x_2) < 0$.  The function is evaluated at the midpoint of these
	/// values and the bound that gives the same sign in the function evaluation is
	/// replaced. The bisection is stopped when the change in the value of $x$ is
	/// below the accuracy, or the evaluation of the function at $x$ is zero.
	/// </para>
	/// </summary>
	public class BisectionSingleRootFinder : RealSingleRootFinder
	{

	  private readonly double _accuracy;
	  private const int MAX_ITER = 100;
	  private const double ZERO = 1e-16;

	  /// <summary>
	  /// Creates an instance.
	  /// Sets the accuracy to 10<sup>-15</sup>.
	  /// </summary>
	  public BisectionSingleRootFinder() : this(1e-15)
	  {
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="accuracy"> The required accuracy of the $x$-position of the root </param>
	  public BisectionSingleRootFinder(double accuracy)
	  {
		_accuracy = Math.Abs(accuracy);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="MathException"> If the root is not found to the required accuracy in 100 attempts </exception>
	  public override double? getRoot(System.Func<double, double> function, double? x1, double? x2)
	  {
		checkInputs(function, x1, x2);
		double y1 = function(x1);
		double y = function(x2);
		if (Math.Abs(y) < _accuracy)
		{
		  return x2;
		}
		if (Math.Abs(y1) < _accuracy)
		{
		  return x1;
		}
		double dx, xRoot, xMid;
		if (y1 < 0)
		{
		  dx = x2 - x1;
		  xRoot = x1.Value;
		}
		else
		{
		  dx = x1 - x2;
		  xRoot = x2.Value;
		}
		for (int i = 0; i < MAX_ITER; i++)
		{
		  dx *= 0.5;
		  xMid = xRoot + dx;
		  y = function(xMid);
		  if (y <= 0)
		  {
			xRoot = xMid;
		  }
		  if (Math.Abs(dx) < _accuracy || Math.Abs(y) < ZERO)
		  {
			return xRoot;
		  }
		}
		throw new MathException("Could not find root in " + MAX_ITER + " attempts");
	  }

	}

}