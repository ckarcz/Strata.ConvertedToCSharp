using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Class that brackets single root of a function. For a 1-D function (<seealso cref="Function"/>) $f(x)$,
	/// initial values for the interval, $x_1$ and $x_2$, are supplied.
	/// <para>
	/// A root is assumed to be bracketed if $f(x_1)f(x_2) < 0$. If this condition is not satisfied, then either
	/// $|f(x_1)| < |f(x_2)|$, in which case the lower value $x_1$ is shifted in the negative $x$ direction, or
	/// the upper value $x_2$ is shifted in the positive $x$ direction. The amount by which to shift is the difference between
	/// the two $x$ values multiplied by a constant ratio (1.6). If a root is not bracketed after 50 attempts, an exception is thrown.
	/// </para>
	/// </summary>
	public class BracketRoot
	{

	  private const double RATIO = 1.6;
	  private const int MAX_STEPS = 50;

	  /// <summary>
	  /// Gets the bracketed roots.
	  /// </summary>
	  /// <param name="f"> The function, not null </param>
	  /// <param name="xLower"> Initial value of lower bracket </param>
	  /// <param name="xUpper"> Initial value of upper bracket </param>
	  /// <returns> The bracketed points as an array, where the first element is the lower bracket and the second the upper bracket. </returns>
	  /// <exception cref="MathException"> If a root is not bracketed in 50 attempts. </exception>
	  public virtual double[] getBracketedPoints(System.Func<double, double> f, double xLower, double xUpper)
	  {
		ArgChecker.notNull(f, "f");
		double x1 = xLower;
		double x2 = xUpper;
		double f1 = 0;
		double f2 = 0;
		f1 = f(x1);
		f2 = f(x2);
		if (double.IsNaN(f1))
		{
		  throw new MathException("Failed to bracket root: function invalid at x = " + x1 + " f(x) = " + f1);
		}
		if (double.IsNaN(f2))
		{
		  throw new MathException("Failed to bracket root: function invalid at x = " + x2 + " f(x) = " + f2);
		}

		for (int count = 0; count < MAX_STEPS; count++)
		{
		  if (f1 * f2 < 0)
		  {
			return new double[] {x1, x2};
		  }
		  if (Math.Abs(f1) < Math.Abs(f2))
		  {
			x1 += RATIO * (x1 - x2);
			f1 = f(x1);
			if (double.IsNaN(f1))
			{
			  throw new MathException("Failed to bracket root: function invalid at x = " + x1 + " f(x) = " + f1);
			}
		  }
		  else
		  {
			x2 += RATIO * (x2 - x1);
			f2 = f(x2);
			if (double.IsNaN(f2))
			{
			  throw new MathException("Failed to bracket root: function invalid at x = " + x2 + " f(x) = " + f2);
			}
		  }
		}
		throw new MathException("Failed to bracket root");
	  }

	  /// <summary>
	  /// Gets the bracketed roots.
	  /// </summary>
	  /// <param name="f"> The function, not null </param>
	  /// <param name="xLower"> Initial value of lower bracket </param>
	  /// <param name="xUpper"> Initial value of upper bracket </param>
	  /// <param name="minX">  the minimum x </param>
	  /// <param name="maxX">  the maximum x </param>
	  /// <returns> The bracketed points as an array, where the first element is the lower bracket and the second the upper bracket. </returns>
	  /// <exception cref="MathException"> If a root is not bracketed in 50 attempts. </exception>
	  public virtual double[] getBracketedPoints(System.Func<double, double> f, double xLower, double xUpper, double minX, double maxX)
	  {
		ArgChecker.notNull(f, "f");
		ArgChecker.isTrue(xLower >= minX, "xLower < minX");
		ArgChecker.isTrue(xUpper <= maxX, "xUpper < maxX");
		double x1 = xLower;
		double x2 = xUpper;
		double f1 = 0;
		double f2 = 0;
		bool lowerLimitReached = false;
		bool upperLimitReached = false;
		f1 = f(x1);
		f2 = f(x2);
		if (double.IsNaN(f1))
		{
		  throw new MathException("Failed to bracket root: function invalid at x = " + x1 + " f(x) = " + f1);
		}
		if (double.IsNaN(f2))
		{
		  throw new MathException("Failed to bracket root: function invalid at x = " + x2 + " f(x) = " + f2);
		}
		for (int count = 0; count < MAX_STEPS; count++)
		{
		  if (f1 * f2 <= 0)
		  {
			return new double[] {x1, x2};
		  }
		  if (lowerLimitReached && upperLimitReached)
		  {
			throw new MathException("Failed to bracket root: no root found between minX and maxX");
		  }
		  if (Math.Abs(f1) < Math.Abs(f2) && !lowerLimitReached)
		  {
			x1 += RATIO * (x1 - x2);
			if (x1 < minX)
			{
			  x1 = minX;
			  lowerLimitReached = true;
			}
			f1 = f(x1);
			if (double.IsNaN(f1))
			{
			  throw new MathException("Failed to bracket root: function invalid at x = " + x1 + " f(x) = " + f1);
			}
		  }
		  else
		  {
			x2 += RATIO * (x2 - x1);
			if (x2 > maxX)
			{
			  x2 = maxX;
			  upperLimitReached = true;
			}
			f2 = f(x2);
			if (double.IsNaN(f2))
			{
			  throw new MathException("Failed to bracket root: function invalid at x = " + x2 + " f(x) = " + f2);
			}
		  }
		}
		throw new MathException("Failed to bracket root: max iterations");
	  }

	}

}