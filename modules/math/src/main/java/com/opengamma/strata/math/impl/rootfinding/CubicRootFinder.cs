using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{
	using DoubleMath = com.google.common.math.DoubleMath;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using RealPolynomialFunction1D = com.opengamma.strata.math.impl.function.RealPolynomialFunction1D;

	/// <summary>
	/// Class that calculates the roots of a cubic equation. 
	/// <para>
	/// As the polynomial has real coefficients, the roots of the cubic can be found using the method described
	/// <a href="http://mathworld.wolfram.com/CubicFormula.html">here</a>.
	/// </para>
	/// </summary>
	public class CubicRootFinder : Polynomial1DRootFinder<ComplexNumber>
	{

	  private static readonly double TWO_PI = 2 * Math.PI;

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="IllegalArgumentException"> If the function is not cubic </exception>
	  public virtual ComplexNumber[] getRoots(RealPolynomialFunction1D function)
	  {
		ArgChecker.notNull(function, "function");
		double[] coefficients = function.Coefficients;
		ArgChecker.isTrue(coefficients.Length == 4, "Function is not a cubic");
		double divisor = coefficients[3];
		double a = coefficients[2] / divisor;
		double b = coefficients[1] / divisor;
		double c = coefficients[0] / divisor;
		double aSq = a * a;
		double q = (aSq - 3 * b) / 9;
		double r = (2 * a * aSq - 9 * a * b + 27 * c) / 54;
		double rSq = r * r;
		double qCb = q * q * q;
		double constant = a / 3;
		if (rSq < qCb)
		{
		  double mult = -2 * Math.Sqrt(q);
		  double theta = Math.Acos(r / Math.Sqrt(qCb));
		  return new ComplexNumber[]
		  {
			  new ComplexNumber(mult * Math.Cos(theta / 3) - constant, 0),
			  new ComplexNumber(mult * Math.Cos((theta + TWO_PI) / 3) - constant, 0),
			  new ComplexNumber(mult * Math.Cos((theta - TWO_PI) / 3) - constant, 0)
		  };
		}
		double s = -Math.Sign(r) * Math.cbrt(Math.Abs(r) + Math.Sqrt(rSq - qCb));
		double t = DoubleMath.fuzzyEquals(s, 0d, 1e-16) ? 0 : q / s;
		double sum = s + t;
		double real = -0.5 * sum - constant;
		double imaginary = Math.Sqrt(3) * (s - t) / 2;
		return new ComplexNumber[]
		{
			new ComplexNumber(sum - constant, 0),
			new ComplexNumber(real, imaginary),
			new ComplexNumber(real, -imaginary)
		};
	  }

	}

}