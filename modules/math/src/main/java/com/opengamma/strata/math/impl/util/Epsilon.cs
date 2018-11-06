using System;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.util
{
	/// <summary>
	/// Taylor expansion epsilon.
	/// </summary>
	// CSOFF: AbbreviationAsWordInName
	public sealed class Epsilon
	{

	  // Coefficients for the Taylor expansion of (e^x-1)/x and its first two derivatives
	  private static readonly double[] COEFF1 = new double[] {1 / 24.0, 1 / 6.0, 1 / 2.0, 1};
	  private static readonly double[] COEFF2 = new double[] {1 / 144.0, 1 / 30.0, 1 / 8.0, 1 / 3.0, 1 / 2.0};
	  private static readonly double[] COEFF3 = new double[] {1 / 168.0, 1 / 36.0, 1 / 10.0, 1 / 4.0, 1 / 3.0};

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// This is the Taylor expansion of $$\frac{\exp(x)-1}{x}$$ - note for $$|x| > 10^{-10}$$ the expansion is note used .
	  /// </summary>
	  /// <param name="x">  the value </param>
	  /// <returns> the result  </returns>
	  public static double epsilon(double x)
	  {
		if (Math.Abs(x) > 1e-10)
		{
		  return Math.expm1(x) / x;
		}
		return taylor(x, COEFF1);
	  }

	  /// <summary>
	  /// This is the Taylor expansion of the first derivative of $$\frac{\exp(x)-1}{x}$$.
	  /// </summary>
	  /// <param name="x">  the value </param>
	  /// <returns> the result  </returns>
	  public static double epsilonP(double x)
	  {
		if (Math.Abs(x) > 1e-7)
		{
		  return ((x - 1) * Math.expm1(x) + x) / x / x;
		}
		return taylor(x, COEFF2);
	  }

	  /// <summary>
	  /// This is the Taylor expansion of the second derivative of $$\frac{\exp(x)-1}{x}$$.
	  /// </summary>
	  /// <param name="x">  the value </param>
	  /// <returns> the result  </returns>
	  public static double epsilonPP(double x)
	  {
		if (Math.Abs(x) > 1e-5)
		{
		  double x2 = x * x;
		  double x3 = x * x2;
		  return (Math.expm1(x) * (x2 - 2 * x + 2) + x2 - 2 * x) / x3;
		}
		return taylor(x, COEFF3);
	  }

	  private static double taylor(double x, double[] coeff)
	  {
		double sum = coeff[0];
		int n = coeff.Length;
		for (int i = 1; i < n; i++)
		{
		  sum = coeff[i] + x * sum;
		}
		return sum;
	  }

	  //-------------------------------------------------------------------------
	  // restricted constructor
	  private Epsilon()
	  {
	  }

	}

}