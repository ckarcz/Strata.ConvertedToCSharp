using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using RealPolynomialFunction1D = com.opengamma.strata.math.impl.function.RealPolynomialFunction1D;

	/// <summary>
	/// Class that calculates the real roots of a quadratic function. 
	/// <para>
	/// The roots can be found analytically. For a quadratic $ax^2 + bx + c = 0$, the roots are given by:
	/// $$
	/// \begin{align*}
	/// x_{1, 2} = \frac{-b \pm \sqrt{b^2 - 4ac}}{2a}
	/// \end{align*}
	/// $$
	/// If no real roots exist (i.e. $b^2 - 4ac < 0$) then an exception is thrown.
	/// </para>
	/// </summary>
	public class QuadraticRealRootFinder : Polynomial1DRootFinder<double>
	{

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="IllegalArgumentException"> If the function is not a quadratic </exception>
	  /// <exception cref="MathException"> If the roots are not real </exception>
	  public virtual double?[] getRoots(RealPolynomialFunction1D function)
	  {
		ArgChecker.notNull(function, "function");
		double[] coefficients = function.Coefficients;
		ArgChecker.isTrue(coefficients.Length == 3, "Function is not a quadratic");
		double c = coefficients[0];
		double b = coefficients[1];
		double a = coefficients[2];
		double discriminant = b * b - 4 * a * c;
		if (discriminant < 0)
		{
		  throw new MathException("No real roots for quadratic");
		}
		double q = -0.5 * (b + Math.Sign(b) * discriminant);
		return new double?[] {q / a, c / q};
	  }

	}

}